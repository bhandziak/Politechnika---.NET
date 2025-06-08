using Microsoft.AspNetCore.Mvc;

namespace CarWorkshopProjekt.Helpers
{
    public class AuthHeaderHelper : IAuthHeaderHelper
    {
        public bool TryGetUserId(HttpRequest request, out Guid userId, out IActionResult errorResult)
        {
            userId = Guid.Empty;
            errorResult = null;
            //Szukanie headera
            if (!request.Headers.TryGetValue("auth", out var authHeader))
            {
                errorResult = new UnauthorizedObjectResult(new { message = "Missing auth header" });
                return false;
            }
            //Sprawdzenie formatu GUID
            if (!Guid.TryParse(authHeader, out userId))
            {
                errorResult = new BadRequestObjectResult(new { message = "Invalid auth GUID" });
                return false;
            }
            return true;
        }
    }
}
