using CarWorkshopProjekt.DTOs;

namespace CarWorkshopProjekt.Services
{
    public interface IServiceOrderService
    {
        Task<List<ReturnServiceOrder>> GetAllAsync();

    }
}
