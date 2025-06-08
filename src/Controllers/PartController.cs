using Microsoft.AspNetCore.Mvc;
using CarWorkshopProjekt.Data;
using CarWorkshopProjekt.DTOs;
using Microsoft.AspNetCore.Authorization;
using CarWorkshopProjekt.Mappers;
using Microsoft.EntityFrameworkCore;
namespace CarWorkshopProjekt.Controllers
{
    [ApiController]
    [Route("api/part")]
    public class PartController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly PartMapper _partMapper = new(); //Mapperly


        public PartController(AppDbContext context)
        {
            _context = context;
        }


        // GET: api/part/getAll
        [Authorize(Roles = "admin,mechanic")]
        [HttpGet("getAll")]
        public async Task<ActionResult> GetAll()
        {
            var parts = await _context.Parts.ToListAsync(); // pobranie encji
            var result = _partMapper.ToReturnDtoList(parts); //mapowanie na dto

            return Ok(result);
        }

        // POST: api/part/addPart
        [Authorize(Roles = "admin,receptionist")]
        [HttpPost("addPart")]
        public async Task<ActionResult> AddPart([FromBody] AddPart partDTO)
        {

            //stworzenie nowego 'part' przez mapperly
            var newPart = _partMapper.MapToEntity(partDTO);
            if(newPart == null)
            {
                return BadRequest("Podano niepoprawne dane - sprawdź format ceny jednostkowej");
            }
            //dodanie part do bazy
            _context.Parts.Add(newPart);
            await _context.SaveChangesAsync();

            return Ok("Sukces");
        }

        // PUT: api/part/update/{partId}
        [Authorize(Roles = "admin,receptionist")]
        [HttpPut("update/{partId}")]
        public async Task<IActionResult> UpdatePart(Guid partId, [FromBody] AddPart partDTO)
        {
            var part = await _context.Parts.FindAsync(partId);

            if (part == null)
            {
                return NotFound("Część o podanym ID nie istnieje.");
            }
            //Zmiana UnitPrice na decimal
            if (!decimal.TryParse(partDTO.UnitPrice, out var parsedPrice))
            {
                return BadRequest("Nieprawidłowa wartość ceny.");
            }
            //Mapperly
            //update part przez mapperly
            _partMapper.UpdatePart(partDTO, part);

            await _context.SaveChangesAsync();
            return Ok("Sukces");
        }
        
        // DELETE: api/part/delete/{partId}
        [Authorize(Roles = "admin,receptionist")]
        [HttpDelete("delete/{partId}")]
        public async Task<IActionResult> DeletePart(Guid partId)
        {
            var part = await _context.Parts.FindAsync(partId);

            if (part == null)
            {
                return NotFound("Część o podanym ID nie istnieje.");
            }

            _context.Parts.Remove(part);
            try
            {
                await _context.SaveChangesAsync();
                return Ok("Część została pomyślnie usunięta.");
            }
            catch (DbUpdateException ex)
            {
                //sprawdzenie czy wyjątek dotyczy referencji
                if (ex.InnerException != null && ex.InnerException.Message.Contains("REFERENCE"))
                {
                    return BadRequest("Nie można usunąć part, ponieważ istnieją z nią powiązane dane.");
                }

                // Inny wyjątek - ogólny komunikat
                return StatusCode(500, "Wystąpił błąd podczas usuwania części.");
            }
        }


    }
}
