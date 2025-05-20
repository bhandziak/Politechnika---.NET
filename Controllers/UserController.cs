using Microsoft.AspNetCore.Mvc;
using CarWorkshopProjekt.Data;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using CarWorkshopProjekt.Helpers;
using CarWorkshopProjekt.DTOs;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Identity;
namespace CarWorkshopProjekt.Controllers
{
    [ApiController]
    [Route("api/user")]
    public class UserController:ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly AppDbContext _context;
        private readonly IAuthHeaderHelper _authHeaderHelper;

        public UserController(ILogger<UserController> logger, AppDbContext context, IAuthHeaderHelper authHeaderHelper)
        {
            _logger = logger;
            _context = context;
            _authHeaderHelper = authHeaderHelper;
        }
        // POST: api/user/register
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] DTOs.RegisterRequest newUserDto)
        {
            var userRegex = new Regex(@"^[a-zA-Z][a-zA-Z0-9\-_#]{4,24}$");
            var passRegex = new Regex(@"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*\-]).{8,64}$");

            if (string.IsNullOrWhiteSpace(newUserDto.Login) || !userRegex.IsMatch(newUserDto.Login))
                return BadRequest("Nieprawidłowy login. Musi zaczynać się od litery, mieć 5-25 znaków i zawierać tylko litery i cyfry.");

            if (string.IsNullOrWhiteSpace(newUserDto.Password) || !passRegex.IsMatch(newUserDto.Password))
                return BadRequest("Nieprawidłowe hasło. Musi mieć 8-64 znaki, zawierać wielką literę, małą literę, cyfrę i znak specjalny.");

            // Sprawdź, czy użytkownik istnieje w bazie
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Login == newUserDto.Login);
            if (existingUser != null)
                return Conflict("Użytkownik o takim loginie już istnieje.");

            // Hashowanie hasła własną metodą
            string hashedPassword = PasswordHasher.HashPassword(newUserDto.Password);

            // Stwórz nowego użytkownika
            var newUser = new User
            {
                UserId = Guid.NewGuid(),
                Login = newUserDto.Login,
                Password = hashedPassword
            };

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Użytkownik zarejestrowany pomyślnie." });
        }

        // POST: api/user/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] DTOs.LoginRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Login) || string.IsNullOrWhiteSpace(request.Password))
                return BadRequest("Login i hasło są wymagane.");

            // Znajdź użytkownika po loginie
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Login == request.Login);
            if (user == null)
                return Unauthorized("Nieprawidłowy login lub hasło.");

            // Sprawdź hasło za pomocą klasy PasswordHasher
            bool isValid = PasswordHasher.VerifyPassword(request.Password, user.Password);
            if (!isValid)
                return Unauthorized("Nieprawidłowy login lub hasło.");

            var userToSend = new
            {
                Login = user.Login,
                Role = user.Role,
                Id = user.UserId
            };

            return Ok(new
            {
                message = "Zalogowano pomyślnie.",
                user = userToSend
            });
        }

        // GET: api/user/getAllUsers
        [HttpGet("getAllUsers")]
        public IActionResult GetAllUsers()
        {
            //Pobranie headera i sprawdzenie czy się zgadza
            if (!_authHeaderHelper.TryGetUserId(Request, out Guid userId, out IActionResult error))
                return error;
            
            var verified = UserVerification.VerifyUser(userId, _context, "admin");//sprawdzenie czy role=admin
            if (!verified)
            {
                return Forbid("Użytkownik nie ma uprawnień do wyświetlenia zasobu");
            }

            var users = _context.Users
                .Select(u => new ReturnUser
                {
                    UserId = u.UserId,
                    Login = u.Login,
                    Role = u.Role
                }).ToList();
            return Ok(new { Users = users });
        }

        // PUT: api/user/setRole/{userId}
        [HttpPut("setRole/{userId}")]
        public IActionResult SetRole(Guid userId, [FromBody] RoleUpdateRequest request)
        {
            // Weryfikacja użytkownika
            //Pobranie headera i sprawdzenie czy się zgadza
            if (!_authHeaderHelper.TryGetUserId(Request, out Guid thisuserId, out IActionResult error))
                return error;

            var verified = UserVerification.VerifyUser(thisuserId, _context, "admin");//sprawdzenie czy role=admin
            if (!verified)
            {
                return Forbid("Użytkownik nie ma uprawnień do zmiany roli");
            }

            //Szukanie użytkownia w bazie do zmiany roli
            var user = _context.Users.FirstOrDefault(u => u.UserId == userId);
            if (user == null)
            {
                return NotFound($"Nie znaleziono użytkownika o Id = {userId}");
            }

            //Aktualizacja roli
            user.Role = request.Role;

            _context.SaveChanges();

            //Odpowiedz servera
            return Ok(new { Message = $"Rola użytkownika {user.Login} została zmieniona na '{request.Role}'." });
        }


    }
}
