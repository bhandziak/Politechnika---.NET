using Microsoft.AspNetCore.Mvc;
using CarWorkshopProjekt.Data;
using CarWorkshopProjekt.DTOs;
using Microsoft.AspNetCore.Authorization;
using CarWorkshopProjekt.Mappers;
using Microsoft.EntityFrameworkCore;
namespace CarWorkshopProjekt.Controllers
{
    [ApiController]
    [Route("api/vehicle")]
    public class VehicleController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly VehicleMapper _vehicleMapper = new(); //Mapperly


        public VehicleController(AppDbContext context)
        {
            _context = context;
        }

        // PUT: api/vehicle/update
        [Authorize(Roles = "admin,receptionist")]
        [HttpPut("update")]
        public async Task<IActionResult> UpdateVehicle([FromBody] UpdateVehicle updateVehicle)
        {
            var vehicle = await _context.Vehicles.FindAsync(updateVehicle.VehicleId);

            if (vehicle == null)
            {
                return NotFound("Samochód o podanym ID nie istnieje.");
            }

            //Mapperly
            //update Vehicle przez mapperly
            _vehicleMapper.UpdateVehicle(updateVehicle, vehicle);

            await _context.SaveChangesAsync();
            return Ok();
        }

        // DELETE: api/vehicle/delete/{vehicleId}
        [Authorize(Roles = "admin,receptionist")]
        [HttpDelete("delete/{vehicleId}")]
        public async Task<IActionResult> DeleteVehicle(Guid vehicleId)
        {
            var vehicle = await _context.Vehicles.FindAsync(vehicleId);

            if (vehicle == null)
            {
                return NotFound("Samochód o podanym ID nie istnieje.");
            }

            _context.Vehicles.Remove(vehicle);
            try
            {
                await _context.SaveChangesAsync();
                return Ok("Samochód został pomyślnie usunięty.");
            }
            catch (DbUpdateException ex)
            {
                //sprawdzenie czy wyjątek dotyczy referencji
                if (ex.InnerException != null && ex.InnerException.Message.Contains("REFERENCE"))
                {
                    return BadRequest("Nie można usunąć pojazdu, ponieważ istnieją z nim powiązane dane (zlecenia).");
                }

                // Inny wyjątek - ogólny komunikat
                return StatusCode(500, "Wystąpił błąd podczas usuwania pojazdu.");
            }
        }


    }
}
