using CarWorkshopProjekt.Data;
using CarWorkshopProjekt.Helpers;
using CarWorkshopProjekt.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Win32;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;

namespace CarWorkshopProjekt.Controllers
{
    [ApiController]
    [Route("api/customer")]
    public class CustomerController : ControllerBase
    {
        private readonly ILogger<CustomerController> _logger;
        private readonly AppDbContext _context;
        private readonly IAuthHeaderHelper _authHeaderHelper;

        public CustomerController(ILogger<CustomerController> logger, AppDbContext context, IAuthHeaderHelper authHeaderHelper)
        {
            _logger = logger;
            _context = context;
            _authHeaderHelper = authHeaderHelper;
        }

        // GET: api/customer/GetCustomers
        [HttpGet("GetCustomers")]
        public ActionResult<IEnumerable<Customer>> GetCustomers()
        {
            var customers = _context.Customers.ToList();
            return Ok(customers);
        }
        // POST: api/customer/addCustomer
        [HttpPost("addCustomer")]
        public async Task<IActionResult> AddCustomer([FromBody] AddCustomer newCustomerDto)
        {
            // Weryfikacja użytkownika
            //Pobranie headera i sprawdzenie czy się zgadza
            if (!_authHeaderHelper.TryGetUserId(Request, out Guid thisuserId, out IActionResult error))
                return error;

            var verified = UserVerification.VerifyUser(thisuserId, _context, "receptionist");//sprawdzenie czy role=receptionist
            if (!verified)
            {
                return Forbid("Użytkownik nie ma uprawnień do dodania nowego klienta");
            }

            var nameRegex = new Regex(@"^[A-ZĄĆĘŁŃÓŚŹŻ][a-ząćęłńóśźż]{1,29}$");
            var surnameRegex = new Regex(@"^[A-ZĄĆĘŁŃÓŚŹŻ][a-ząćęłńóśźż\-]{1,49}$");
            var phoneRegex = new Regex(@"^\+48\d{9}$");
            if (string.IsNullOrWhiteSpace(newCustomerDto.FirstName) || !nameRegex.IsMatch(newCustomerDto.FirstName))
                return BadRequest("Imię musi zaczynać się wielką literą i zawierać tylko litery.");

            if (string.IsNullOrWhiteSpace(newCustomerDto.LastName) || !surnameRegex.IsMatch(newCustomerDto.LastName))
                return BadRequest("Nazwisko musi zaczynać się wielką literą i zawierać tylko litery lub myślnik.");

            if (string.IsNullOrWhiteSpace(newCustomerDto.PhoneNumber) || !phoneRegex.IsMatch(newCustomerDto.PhoneNumber))
                return BadRequest("Numer telefonu musi być w formacie +48123456789.");

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

        // POST: api/customer/{customerID}/addVehicle
        [HttpPost("{customerID}/addVehicle")]
        public async Task<IActionResult> AddVehicle(Guid customerID, [FromBody] AddVehicle newVehicleDto)
        {
            // Weryfikacja użytkownika
            //Pobranie headera i sprawdzenie czy się zgadza
            if (!_authHeaderHelper.TryGetUserId(Request, out Guid thisuserId, out IActionResult error))
                return error;

            var verified = UserVerification.VerifyUser(thisuserId, _context, "receptionist");//sprawdzenie czy role=receptionist
            if (!verified)
            {
                return Forbid("Użytkownik nie ma uprawnień do dodania nowego pojazdu klienta");
            }

            //Szukanie klienta w bazie
            var customer = _context.Customers.FirstOrDefault(c => c.CustomerId == customerID);
            if (customer == null)
            {
                return NotFound($"Nie znaleziono użytkownika o Id = {customerID}");
            }

            //Sprawdzenie poprawności danych samochodu
            var brandRegex = new Regex(@"^[A-Z][a-zA-Z\s\-]{1,29}$");
            var modelRegex = new Regex(@"^[A-Za-z0-9\s\-]{1,30}$");
            var vinRegex = new Regex(@"^[A-HJ-NPR-Z0-9]{17}$");
            var registralNumberRegex = new Regex(@"^[A-Z]{2,3}\s?\d{4,5}[A-Z]{0,2}$");
            if (string.IsNullOrWhiteSpace(newVehicleDto.BrandVehicle) || !brandRegex.IsMatch(newVehicleDto.BrandVehicle))
                return BadRequest("Marka musi zaczynać się wielką literą i zawierać tylko litery, spacje lub myślniki.");
            if (string.IsNullOrWhiteSpace(newVehicleDto.ModelVehicle) || !modelRegex.IsMatch(newVehicleDto.ModelVehicle))
                return BadRequest("Model może zawierać litery, cyfry, spacje i myślniki.");
            if (string.IsNullOrWhiteSpace(newVehicleDto.VINVehicle) || !vinRegex.IsMatch(newVehicleDto.VINVehicle))
                return BadRequest("VIN musi składać się z dokładnie 17 znaków (bez I, O, Q).");
            if (string.IsNullOrWhiteSpace(newVehicleDto.RegistralNumberVehicle) || !registralNumberRegex.IsMatch(newVehicleDto.RegistralNumberVehicle))
                return BadRequest("Numer rejestracyjny ma nieprawidłowy format.");
            if (newVehicleDto.YearVehicle < 1850 || newVehicleDto.YearVehicle > 2100)
                return BadRequest("Rok musi być w zakresie 1850–2100.");

            //Sprawdzenie, po nr VIN czy klient ma juz taki samochod w bazie (po ServiceOrders, a potem po VINie wszystkich samochodów klienta)
            var existingCustomer = await _context.Customers
                .Include(c => c.ServiceOrders)
                .ThenInclude(so => so.Vehicle)
                .FirstOrDefaultAsync(c =>
                    c.CustomerId == customerID  && //ogranicza zapytanie do szukania tylko dla tego klienta któremu dodajemy samochód
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
                ImageURL = null
            };

            _context.Vehicles.Add(newVehicle);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Samochód dodany pomyślnie." });
        }

        }
    }
