using CarWorkshopProjekt.DTOs;

namespace CarWorkshopProjekt.Services
{
    public interface IServiceOrderService
    {
        Task<List<ReturnServiceOrder>> GetAllAsync();
        Task<List<ReturnMechanicsTasks>> GetServiceTasksWithPartsAsync(Guid orderId);
    }
}
