using CarWorkshopProjekt.Data;
using CarWorkshopProjekt.DTOs;
using CarWorkshopProjekt.Mappers;
using Microsoft.EntityFrameworkCore;

namespace CarWorkshopProjekt.Services
{
    public class ServiceOrderService : IServiceOrderService
    {
        private readonly AppDbContext _context;

        public ServiceOrderService(AppDbContext context)
        {
            _context = context;
        }


        public async Task<List<ReturnServiceOrder>> GetAllAsync()
        {
            var serviceOrders = await _context.ServiceOrders
                .Include(so => so.Customer)
                .Include(so => so.Vehicle)
                .Include(so => so.User)
                .ToListAsync();

           return ServiceOrderMapper.ToReturnDtoList(serviceOrders);
        }
    }
}
