using Microsoft.AspNetCore.Mvc;
using CarWorkshopProjekt.Data;
using CarWorkshopProjekt.DTOs;
using Microsoft.AspNetCore.Authorization;
using CarWorkshopProjekt.Mappers;
using Microsoft.EntityFrameworkCore;
namespace CarWorkshopProjekt.Controllers
{
    [ApiController]
    [Route("api/serviceTask")]
    public class ServiceTaskController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly PartMapper _partMapper = new(); //Mapperly


        public ServiceTaskController(AppDbContext context)
        {
            _context = context;
        }

        //TESTY
        // PUT: api/serviceTask/setPart
        [Authorize(Roles = "admin,mechanic")]
        [HttpPut("setPart")]
        public async Task<ActionResult<decimal>> GetAll([FromBody] SetPartDTO partDTO)
        {
            //Konwertowanie danych wejściowych na GUID
            if (!Guid.TryParse(partDTO.ServiceTaskId, out Guid serviceTaskId))
                return BadRequest("Nieprawidłowy format ID zadania serwisowego.");

            if (!Guid.TryParse(partDTO.partId, out Guid partId))
                return BadRequest("Nieprawidłowy format ID części.");

            //Szukanie ServiceTask
            var serviceTask = await _context.ServiceTasks
                .Include(st => st.UsedParts)
                .FirstOrDefaultAsync(st => st.ServiceTaskId == serviceTaskId);

            if (serviceTask == null)
                return NotFound("Nie znaleziono service task");

            //Szukanie części 'part'
            var part = await _context.Parts.FindAsync(partId);
            if (part == null)
                return NotFound("Nie znaleziono części");

            //Tworzneie nowej części 'usedPart'
            var usedPart = new UsedPart
            {
                UsedPartId = Guid.NewGuid(),
                ServiceTaskId = serviceTaskId,
                PartId = partId,
                Quantity = int.Parse(partDTO.quantity)
            };

            //Dodanie 'usedPart' do pobranego 'serviceTask'
            _context.UsedParts.Add(usedPart);

            //Licznie 'totalPrice' i zwracanie w wyniku zapytania
            var totalPrice = part.UnitPrice * int.Parse(partDTO.quantity) + serviceTask.LaborCost;

            await _context.SaveChangesAsync();

            return Ok(totalPrice);
        }


    }
}
