using Microsoft.AspNetCore.Mvc;

namespace CarWorkshopProjekt.Helpers
{
    public interface IAuthHeaderHelper
    {
        bool TryGetUserId(HttpRequest request, out Guid userId, out IActionResult errorResult);
    }
}
