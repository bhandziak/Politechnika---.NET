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
            var serviceTask = await _context.ServiceTasks
                .Include(st => st.UsedParts)
                .FirstOrDefaultAsync(st => st.ServiceTaskId == Guid.Parse(partDTO.ServiceTaskId));

            if (serviceTask == null)
                return NotFound("Nie znaleziono service task");

            var part = await _context.Parts.FindAsync(partDTO.partId);
            if (part == null)
                return NotFound("Nie znaleziono części");

            var usedPart = new UsedPart
            {
                UsedPartId = Guid.NewGuid(),
                ServiceTaskId = serviceTask.ServiceTaskId,
                PartId = part.PartId,
                Quantity = int.Parse(partDTO.quantity)
            };

            serviceTask.UsedParts.Add(usedPart);

            var totalPrice = part.UnitPrice * int.Parse(partDTO.quantity) + serviceTask.LaborCost;

            await _context.SaveChangesAsync();

            return Ok(totalPrice);
        }


    }
}
