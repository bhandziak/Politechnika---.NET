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
using CarWorkshopProjekt.Services;
namespace CarWorkshopProjekt.Controllers
{
    [ApiController]
    [Route("api/user")]
    public class UserController:ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IUserService _userService; // Services/IUserService
        private readonly ILogger<UserController> _logger;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserController(
            AppDbContext context,
            IUserService userService, // Services
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
            _userService = userService; // Services
        }

        // POST: api/user/register
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] DTOs.RegisterRequest newUserDto)
        {
            // Services
            var result = await _userService.RegisterUserAsync(newUserDto);

            if (!result.Success)
                return BadRequest(result.ErrorMessage);

            return Ok(new { message = "Użytkownik zarejestrowany pomyślnie." });
        }

        // POST: api/user/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] DTOs.LoginRequest request)
        {
            // Services
            var result = await _userService.LoginAsync(request);

            if (!result.Success)
                return Unauthorized(result.ErrorMessage);

            return Ok(new
            {
                message = "Zalogowano pomyślnie.",
                user = result.UserInfo
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
            // Services
            var (success, message) = await _userService.SetUserRoleAsync(userId, request.Role);

            if (!success)
                return StatusCode(500, message);

            return Ok(new { Message = message });
        }
    }
}
