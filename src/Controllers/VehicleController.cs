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
            return Ok("Samochód został zaktualizowany");
        }

        // DELETE: api/vehicle/delete/{vehicleId}
        [Authorize(Roles = "admin,receptionist")]
        [HttpDelete("delete/{vehicleId}")]
        public async Task<IActionResult> DeleteVehicle(Guid vehicleId)
        {
            var vehicle = await _context.Vehicles // wyszukanie całego obiektu wraz z obj Comments, ServiceOrders
                .Include(v => v.ServiceOrders)
                .ThenInclude(so => so.Comments)
                .Include(v => v.ServiceOrders)
                .ThenInclude(so => so.ServiceTasks)
                .FirstOrDefaultAsync(v => v.VehicleId == vehicleId);

            if (vehicle == null)
            {
                return NotFound("Samochód o podanym ID nie istnieje.");
            }
            // można usunąć Vehicles jeśli ServiceOrders jeszcze nie został stworzony przez recepcjoniste
            bool allOrdersDeletable = vehicle.ServiceOrders.All(so => so.StatusOrder == null);
            if (!allOrdersDeletable)
            {
                return BadRequest("Nie można usunąć pojazdu, ponieważ istnieją z nim powiązane dane (zlecenia).");
            }

            // kaskadowe usuwanie ServiceOrders
            foreach (var serviceOrder in vehicle.ServiceOrders)
            {
                _context.Comments.RemoveRange(serviceOrder.Comments);
                _context.ServiceTasks.RemoveRange(serviceOrder.ServiceTasks);
            }

            _context.ServiceOrders.RemoveRange(vehicle.ServiceOrders);
            _context.Vehicles.Remove(vehicle);

            try
            {
                await _context.SaveChangesAsync();
                return Ok("Pojazd został usunięty.");
            }
            catch (Exception)
            {
                return StatusCode(500, "Błąd przy usuwaniu pojazdu.");
            }
        }


    }
}
