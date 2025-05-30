using CarWorkshopProjekt.Data;
using CarWorkshopProjekt.Helpers;
using CarWorkshopProjekt.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Win32;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace CarWorkshopProjekt.Controllers
{
    [ApiController]
    [Route("api/customer")]
    public class CustomerController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ILogger<UserController> _logger;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public CustomerController(
            AppDbContext context,
            ILogger< UserController > logger,
            UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _logger = logger;
            _userManager = userManager;
            _roleManager = roleManager;
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
            // Walidacja pliku
            if (photo == null || photo.Length == 0)
                return BadRequest("Plik jest pusty.");

            var ext = Path.GetExtension(photo.FileName).ToLowerInvariant();
            var allowedExtensions = new[] { ".jpg", ".png" };

            if (!allowedExtensions.Contains(ext))
                return BadRequest("Dozwolone formaty: JPG, PNG.");

            // Znajdź pojazd
            var vehicle = await _context.Vehicles.FindAsync(vehicleId);
            if (vehicle == null)
                return NotFound("Pojazd nie istnieje.");

            // Zapis pliku
            var fileName = $"{Guid.NewGuid()}{ext}";
            var uploadPath = Path.Combine("wwwroot", "uploads");
            Directory.CreateDirectory(uploadPath);
            var filePath = Path.Combine(uploadPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await photo.CopyToAsync(stream);
            }

            // Zapis ścieżki
            vehicle.ImageURL = $"/uploads/{fileName}";
            await _context.SaveChangesAsync();

            return Ok(new { message = "Obraz zapisany", imageUrl = vehicle.ImageURL });
        }

    }
}
