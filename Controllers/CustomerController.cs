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

namespace CarWorkshopProjekt.Controllers
{
    [ApiController]
    [Route("api/customer")]
    public class CustomerController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ICustomerService _validationService; // Services/ICustomerValidationService

        public CustomerController(
            AppDbContext context,
            ICustomerService validationService // Services
            )
        {
            _context = context;
            _validationService = validationService; // Services
        }

        // GET: api/customer/getCustomers
        [Authorize(Roles = "admin,receptionist,user,mechanic")]
        [HttpGet("getCustomers")]
        public ActionResult<IEnumerable<Customer>> GetCustomers()
        {
            var customers = _context.Customers
            .Select(c => new ReturnCustomer//DTO dla zwrócenia danych klienta
            {
                CustomerId = c.CustomerId,
                NameCustomer = c.NameCustomer,
                SurnameCustomer = c.SurnameCustomer,
                PhoneNumber = c.PhoneNumber
            })
            .ToList();
            return Ok(customers);
        }

        // POST: api/customer/addCustomer
        [Authorize(Roles = "admin,receptionist")]
        [HttpPost("addCustomer")]
        public async Task<IActionResult> AddCustomer([FromBody] AddCustomer newCustomerDto)
        {
            // Services
            if (!_validationService.IsValidFirstName(newCustomerDto.FirstName, out var firstNameError))
                return BadRequest(firstNameError);

            if (!_validationService.IsValidLastName(newCustomerDto.LastName, out var lastNameError))
                return BadRequest(lastNameError);

            if (!_validationService.IsValidPhoneNumber(newCustomerDto.PhoneNumber, out var phoneError))
                return BadRequest(phoneError);

            //sprawdzenie, czy klient istnieje w bazie (po nr telefonu)
            var existingCustomer = await _context.Customers.FirstOrDefaultAsync(c => c.PhoneNumber == newCustomerDto.PhoneNumber);
            if (existingCustomer != null)
                return Conflict("Klient o takim nr. telefonu już istnieje.");

            //stworzenie nowego klienta
            var newCustomer = new Customer
            {
                CustomerId = Guid.NewGuid(),
                NameCustomer = newCustomerDto.FirstName,
                SurnameCustomer = newCustomerDto.LastName,
                PhoneNumber = newCustomerDto.PhoneNumber,
            };

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
            if (!_validationService.IsValidBrand(newVehicleDto.BrandVehicle, out var brandError))
                return BadRequest(brandError);

            if (!_validationService.IsValidModel(newVehicleDto.ModelVehicle, out var modelError))
                return BadRequest(modelError);

            if (!_validationService.IsValidVIN(newVehicleDto.VINVehicle, out var vinError))
                return BadRequest(vinError);

            if (!_validationService.IsValidRegistralNumber(newVehicleDto.RegistralNumberVehicle, out var regError))
                return BadRequest(regError);

            if (!_validationService.IsValidYear(newVehicleDto.YearVehicle, out var yearError))
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
            //Stworzenie nowego samochodu z podanych danych
            var newVehicle = new Vehicle
            {
                VehicleId = Guid.NewGuid(),
                BrandVehicle = newVehicleDto.BrandVehicle,
                ModelVehicle = newVehicleDto.ModelVehicle,
                VINVehicle = newVehicleDto.VINVehicle,
                RegistralNumberVehicle = newVehicleDto.RegistralNumberVehicle,
                YearVehicle = newVehicleDto.YearVehicle,
                ImageURL = "none"
            };

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
            if (!_validationService.IsValidImage(photo, out var error))
                return BadRequest(error);

            // Szukanie pojazdu
            var vehicle = await _context.Vehicles.FindAsync(vehicleId);
            if (vehicle == null)
                return NotFound("Pojazd nie istnieje.");

            // Zapis pliku
            var imageUrl = await _validationService.SaveImageAsync(photo);
            vehicle.ImageURL = imageUrl;
            await _context.SaveChangesAsync();

            return Ok(new { message = "Dodano zdjęcie pojazdu", imageUrl = vehicle.ImageURL });
        }

    }
}
