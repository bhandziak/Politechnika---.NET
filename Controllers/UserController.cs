using Microsoft.AspNetCore.Mvc;
using CarWorkshopProjekt.Data;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using CarWorkshopProjekt.Helpers;
using CarWorkshopProjekt.DTOs;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
namespace CarWorkshopProjekt.Controllers
{
    [ApiController]
    [Route("api/user")]
    public class UserController:ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ILogger<UserController> _logger;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserController(
            AppDbContext context,
            ILogger<UserController> logger,
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        // POST: api/user/register
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] DTOs.RegisterRequest newUserDto)
        {
            var userRegex = new Regex(@"^[a-zA-Z][a-zA-Z0-9\-_#]{4,24}$");
            var passRegex = new Regex(@"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*\-]).{8,64}$");

            if (string.IsNullOrWhiteSpace(newUserDto.UserName) || !userRegex.IsMatch(newUserDto.UserName))
                return BadRequest("Nieprawidłowy login. Musi zaczynać się od litery, mieć 5-25 znaków i zawierać tylko litery i cyfry.");

            if (string.IsNullOrWhiteSpace(newUserDto.Password) || !passRegex.IsMatch(newUserDto.Password))
                return BadRequest("Nieprawidłowe hasło. Musi mieć 8-64 znaki, zawierać wielką literę, małą literę, cyfrę i znak specjalny.");

            // Sprawdź, czy użytkownik istnieje w bazie
            var userExists = await _userManager.FindByNameAsync(newUserDto.UserName);
            if (userExists != null)
                return BadRequest("Użytkownik o takim loginie już istnieje.");

            var newUser = new User
            {
                UserName = newUserDto.UserName,
                Email = newUserDto.UserName,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(newUser, newUserDto.Password);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            // nowa rola "user"
            if (!await _roleManager.RoleExistsAsync("admin"))
                await _roleManager.CreateAsync(new IdentityRole("admin"));

            // przypisz role
            await _userManager.AddToRoleAsync(newUser, "admin");

            return Ok(new { message = "Użytkownik zarejestrowany pomyślnie." });
        }

        // POST: api/user/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] DTOs.LoginRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.UserName) || string.IsNullOrWhiteSpace(request.Password))
                return BadRequest("Login i hasło są wymagane.");

            // Znajdź użytkownika
            var user = await _userManager.FindByNameAsync(request.UserName);
            if (user == null)
                return Unauthorized("Nieprawidłowy login lub hasło.");

            // Sprawdź hasło
            var result = await _signInManager.PasswordSignInAsync(user, request.Password, isPersistent: false, lockoutOnFailure: false);
            if (!result.Succeeded)
                return Unauthorized("Nieprawidłowy login lub hasło.");

            var roles = await _userManager.GetRolesAsync(user);
            var role = roles.FirstOrDefault() ?? "user";

            var userToSend = new
            {
                Login = user.UserName,
                Id = user.Id,
                Role = role
            };

            return Ok(new
            {
                message = "Zalogowano pomyślnie.",
                user = userToSend
            });
        }

        // GET: api/user/logout
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok(new { message = "Pomyślnie wylogowano." });
        }

        // GET: api/user/getAllUsers
        [Authorize(Roles = "admin")]
        [HttpGet("getAllUsers")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = _userManager.Users.ToList();
            var userList = new List<object>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                userList.Add(new
                {
                    userId = user.Id,
                    login = user.UserName,
                    role = roles.FirstOrDefault() ?? "none"
                });
            }

            return Ok(new { users = userList });
        }

        // PUT: api/user/setRole/{userId}
        [Authorize(Roles = "admin")]
        [HttpPut("setRole/{userId}")]
        public async Task<IActionResult> SetRole(string userId, [FromBody] RoleUpdateRequest request)
        {

            //Szukanie użytkownia w bazie do zmiany roli
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return NotFound($"Nie znaleziono użytkownika o Id = {userId}");

            // Czy rola istnieje?
            if (!await _roleManager.RoleExistsAsync(request.Role))
                await _roleManager.CreateAsync(new IdentityRole(request.Role));

            // Wszyskie role użytkownika
            var currentRoles = await _userManager.GetRolesAsync(user);

            // Usuwanie starych ról
            var removeResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);
            if (!removeResult.Succeeded)
                return StatusCode(500, "Błąd przy usuwaniu aktualnych ról użytkownika.");

            // Dodanie nowej roli
            var addResult = await _userManager.AddToRoleAsync(user, request.Role);
            if (!addResult.Succeeded)
                return StatusCode(500, "Błąd przy dodawaniu nowej roli użytkownikowi.");

            return Ok(new { Message = $"Rola użytkownika {user.UserName} została zmieniona na '{request.Role}'." });
        }
    }
}
