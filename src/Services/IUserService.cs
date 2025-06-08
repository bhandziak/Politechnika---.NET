namespace CarWorkshopProjekt.Services
{
    using CarWorkshopProjekt.DTOs;
    public interface IUserService
    {
        //[HttpPost("register")] logic
        Task<(bool Success, string ErrorMessage)> RegisterUserAsync(RegisterRequest newUserDto);

        //[HttpPost("login")] logic
        Task<(bool Success, string ErrorMessage, object UserInfo)> LoginAsync(LoginRequest request);

        //[HttpPut("setRole/{userId}")] logic
        Task<(bool Success, string Message)> SetUserRoleAsync(string userId, string newRole);

    }
}
