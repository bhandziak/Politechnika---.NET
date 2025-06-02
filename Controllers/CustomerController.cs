using CarWorkshopProjekt.Data;
using CarWorkshopProjekt.Helpers;
using CarWorkshopProjekt.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Win32;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using CarWorkshopProjekt.Services;
using CarWorkshopProjekt.Mappers;
using static CarWorkshopProjekt.Controllers.ServiceOrderController;

namespace CarWorkshopProjekt.Controllers
{
    [ApiController]
    [Route("api/customer")]
    public class CustomerController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ICustomerService _customerService; // Services/ICustomerService
        private readonly CustomerMapper _customerMapper = new(); //Mapperly customer
        private readonly VehicleMapper _vehicleMapper = new(); //Mapperly vehicle
        public CustomerController(
            AppDbContext context,
            ICustomerService customerService // Services
            )
        {
            _context = context;
            _customerService = customerService; // Services
        }

        // GET: api/customer/getCustomers
        [Authorize(Roles = "admin,receptionist,user,mechanic")]
        [HttpGet("getCustomers")]
        public ActionResult<IEnumerable<Customer>> GetCustomers()
        {
            var customers = _context.Customers.ToList(); // pobranie encji
            var returnDtos = _customerMapper.ToReturnDtoList(customers); // mapowanie do DTO

            return Ok(returnDtos);
        }

        // POST: api/customer/addCustomer
        [Authorize(Roles = "admin,receptionist")]
        [HttpPost("addCustomer")]
        public async Task<IActionResult> AddCustomer([FromBody] AddCustomer newCustomerDto)
        {
            // Services
            if (!_customerService.IsValidFirstName(newCustomerDto.NameCustomer, out var firstNameError))
                return BadRequest(firstNameError);

            if (!_customerService.IsValidLastName(newCustomerDto.SurnameCustomer, out var lastNameError))
                return BadRequest(lastNameError);

            if (!_customerService.IsValidPhoneNumber(newCustomerDto.PhoneNumber, out var phoneError))
                return BadRequest(phoneError);

            //sprawdzenie, czy klient istnieje w bazie (po nr telefonu)
            var existingCustomer = await _context.Customers.FirstOrDefaultAsync(c => c.PhoneNumber == newCustomerDto.PhoneNumber);
            if (existingCustomer != null)
                return Conflict("Klient o takim nr. telefonu już istnieje.");

            //stworzenie nowego klienta przez mapperly
            var newCustomer = _customerMapper.MapToEntity(newCustomerDto);
            newCustomer.CustomerId = Guid.NewGuid(); //ręczne dodanie guid

            _context.Customers.Add(newCustomer);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Klient zarejestrowany pomyślnie." });
        }

        // POST: api/customer/addVehicle/{customerID}
        [Authorize(Roles = "admin,receptionist")]
        [HttpPost("addVehicle/{customerID}")]
        public async Task<IActionResult> AddVehicle(Guid customerID, [FromBody] AddVehicle newVehicleDto)
        {            
            //Szukanie klienta w bazie
            var customer = _context.Customers.FirstOrDefault(c => c.CustomerId == customerID);
            if (customer == null)
            {
                return NotFound($"Nie znaleziono użytkownika o Id = {customerID}");
            }

            // Services
            if (!_customerService.IsValidBrand(newVehicleDto.BrandVehicle, out var brandError))
                return BadRequest(brandError);

            if (!_customerService.IsValidModel(newVehicleDto.ModelVehicle, out var modelError))
                return BadRequest(modelError);

            if (!_customerService.IsValidVIN(newVehicleDto.VINVehicle, out var vinError))
                return BadRequest(vinError);

            if (!_customerService.IsValidRegistralNumber(newVehicleDto.RegistralNumberVehicle, out var regError))
                return BadRequest(regError);

            if (!_customerService.IsValidYear(newVehicleDto.YearVehicle, out var yearError))
                return BadRequest(yearError);


            //Sprawdzenie, po nr VIN czy klient ma juz taki samochod w bazie (po ServiceOrders, a potem po VINie wszystkich samochodów klienta)
            var existingCustomer = await _context.Customers
                .Include(c => c.ServiceOrders)
                .ThenInclude(so => so.Vehicle)
                .FirstOrDefaultAsync(c =>
                    c.CustomerId == customerID && //ogranicza zapytanie do szukania tylko dla tego klienta któremu dodajemy samochód
                    c.ServiceOrders.Any(so => so.Vehicle != null && so.Vehicle.VINVehicle == newVehicleDto.VINVehicle));

            if (existingCustomer != null)
            {
                return Conflict("Klient posiada już taki samochód.");
            }
            //stworzenie nowego klienta przez mapperly
            var newVehicle = _vehicleMapper.MapToEntity(newVehicleDto);            

            newVehicle.ImageURL = "none"; //ręczne dodanie imageUrl
            newVehicle.VehicleId = Guid.NewGuid(); //ręczne dodanie guid
            
            //Stworzenie nowego ServiceOrder z danego nowego samochodu
            var newServiceOrder = new ServiceOrder
            {
                ServiceOrderId = Guid.NewGuid(),
                VehicleId = newVehicle.VehicleId,
                CustomerId = customerID,
                UserId = null,
                StatusOrder = null,
                Description = null,
                DateFinished = null
            };

            _context.Vehicles.Add(newVehicle);
            _context.ServiceOrders.Add(newServiceOrder);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Samochód dodany pomyślnie." });
        }

        // GET: api/customer/getDetails/{customerID}        
        [Authorize(Roles = "admin,receptionist,user,mechanic")]
        [HttpGet("getDetails/{customerID}")]
        public async Task<IActionResult> GetDetailsAsync(Guid customerID)
        {           
            var customer = await _context.Customers
                .Where(c => c.CustomerId == customerID)
                .Select(c => new ReturnAllCustomer
                {
                    CustomerId = c.CustomerId,
                    NameCustomer = c.NameCustomer,
                    SurnameCustomer = c.SurnameCustomer,
                    PhoneNumber = c.PhoneNumber,
                    Vehicles = c.ServiceOrders
                        .Select(so => so.Vehicle)
                        .Distinct()
                        .Select(v => new ReturnVehicle
                {
                    VehicleId = v.VehicleId,
                    BrandVehicle = v.BrandVehicle,
                    ModelVehicle = v.ModelVehicle,
                    VINVehicle = v.VINVehicle,
                    RegistralNumberVehicle = v.RegistralNumberVehicle,
                    YearVehicle = v.YearVehicle,
                    ImageURL = v.ImageURL
                })
                .ToList()
                })
                 .FirstOrDefaultAsync();
            if (customer == null)
                return NotFound();

            return Ok(customer);
        }
        // POST: api/customer/getDetails/addVehicleImage/{vehicleID}
        [Authorize(Roles = "admin,receptionist,user,mechanic")]
        [HttpPost("getDetails/addVehicleImage/{vehicleID}")]
        public async Task<IActionResult> AddVehicleImage(Guid vehicleId, IFormFile photo)
        {
            // Services
            if (!_customerService.IsValidImage(photo, out var error))
                return BadRequest(error);

            // Szukanie pojazdu
            var vehicle = await _context.Vehicles.FindAsync(vehicleId);
            if (vehicle == null)
                return NotFound("Pojazd nie istnieje.");

            // Zapis pliku
            var imageUrl = await _customerService.SaveImageAsync(photo);
            vehicle.ImageURL = imageUrl;
            await _context.SaveChangesAsync();

            return Ok(new { message = "Dodano zdjęcie pojazdu", imageUrl = vehicle.ImageURL });
        }

        // PUT: api/customer/update
        [Authorize(Roles = "admin,receptionist")]
        [HttpPut("update")]
        public async Task<IActionResult> UpdateCustomer([FromBody] UpdateCustomer updateCustomer)
        {
            var customer = await _context.Customers.FindAsync(updateCustomer.CustomerId);

            if (customer == null)
            {
                return NotFound("Klient o podanym ID nie istnieje.");
            }

            //Mapperly
            //update Customer przez mapperly
            _customerMapper.UpdateCustomer(updateCustomer, customer);

            await _context.SaveChangesAsync();
            return Ok();
        }

        // DELETE: api/customer/delete/{customerId}
        [Authorize(Roles = "admin,receptionist")]
        [HttpDelete("delete/{customerId}")]
        public async Task<IActionResult> DeleteCustomer(Guid customerId)
        {
            var customer = await _context.Customers.FindAsync(customerId);

            if (customer == null)
            {
                return NotFound("Klient o podanym ID nie istnieje.");
            }

            _context.Customers.Remove(customer);

            await _context.SaveChangesAsync();
            return Ok();
        }

    }
}
