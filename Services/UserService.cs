using CarWorkshopProjekt.Data;
using CarWorkshopProjekt.DTOs;
using Microsoft.AspNetCore.Identity;
using System.Text.RegularExpressions;

namespace CarWorkshopProjekt.Services
{
    public class UserService: IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        //[HttpPost("register")] logic
        private readonly Regex _userRegex = new(@"^[a-zA-Z][a-zA-Z0-9\-_#]{4,24}$");
        private readonly Regex _passRegex = new(@"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*\-]).{8,64}$");

        public UserService(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
        }


        //[HttpPost("register")] logic
        public async Task<(bool Success, string ErrorMessage)> RegisterUserAsync(RegisterRequest dto)
        {
            if (string.IsNullOrWhiteSpace(dto.UserName) || !_userRegex.IsMatch(dto.UserName))
                return (false, "Nieprawidłowy login. Musi zaczynać się od litery, mieć 5-25 znaków i zawierać tylko litery i cyfry.");

            if (string.IsNullOrWhiteSpace(dto.Password) || !_passRegex.IsMatch(dto.Password))
                return (false, "Nieprawidłowe hasło. Musi mieć 8-64 znaki, zawierać wielką literę, małą literę, cyfrę i znak specjalny.");

            var existingUser = await _userManager.FindByNameAsync(dto.UserName);
            if (existingUser != null)
                return (false, "Użytkownik o takim loginie już istnieje.");

            var newUser = new User
            {
                UserName = dto.UserName,
                Email = dto.UserName,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(newUser, dto.Password);
            if (!result.Succeeded)
                return (false, string.Join("; ", result.Errors.Select(e => e.Description)));

            const string defaultRole = "admin";
            if (!await _roleManager.RoleExistsAsync(defaultRole))
                await _roleManager.CreateAsync(new IdentityRole(defaultRole));

            await _userManager.AddToRoleAsync(newUser, defaultRole);

            return (true, null);
        }

        //[HttpPost("login")] logic
        public async Task<(bool Success, string ErrorMessage, object UserInfo)> LoginAsync(LoginRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.UserName) || string.IsNullOrWhiteSpace(request.Password))
                return (false, "Login i hasło są wymagane.", null);

            var user = await _userManager.FindByNameAsync(request.UserName);
            if (user == null)
                return (false, "Nieprawidłowy login lub hasło.", null);

            var result = await _signInManager.PasswordSignInAsync(user, request.Password, false, false);
            if (!result.Succeeded)
                return (false, "Nieprawidłowy login lub hasło.", null);

            var roles = await _userManager.GetRolesAsync(user);
            var role = roles.FirstOrDefault() ?? "user";

            var userInfo = new
            {
                Login = user.UserName,
                Id = user.Id,
                Role = role
            };

            return (true, null, userInfo);
        }

        //[HttpPut("setRole/{userId}")] logic
        public async Task<(bool Success, string Message)> SetUserRoleAsync(string userId, string newRole)
        {
            //Szukanie użytkownia w bazie do zmiany roli
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return (false, $"Nie znaleziono użytkownika o Id = {userId}");

            // Czy rola istnieje?
            if (!await _roleManager.RoleExistsAsync(newRole))
                await _roleManager.CreateAsync(new IdentityRole(newRole));

            // Wszyskie role użytkownika
            var currentRoles = await _userManager.GetRolesAsync(user);

            // Usuwanie starych ról
            var removeResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);
            if (!removeResult.Succeeded)
                return (false, "Błąd przy usuwaniu aktualnych ról użytkownika.");

            // Dodanie nowej roli
            var addResult = await _userManager.AddToRoleAsync(user, newRole);
            if (!addResult.Succeeded)
                return (false, "Błąd przy dodawaniu nowej roli użytkownikowi.");

            return (true, $"Rola użytkownika {user.UserName} została zmieniona na '{newRole}'.");
        }
    }
}
