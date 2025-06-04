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

       public async Task<List<ReturnMechanicsTasks>> GetServiceTasksWithPartsAsync(Guid orderId)
        {
            var serviceOrder = await _context.ServiceOrders
                .Include(so => so.ServiceTasks)
                    .ThenInclude(st => st.UsedParts)
                        .ThenInclude(up => up.Part)
                .FirstOrDefaultAsync(so => so.ServiceOrderId == orderId);

            if (serviceOrder == null)
            {
                return null;
            }

            var serviceTasksDto = serviceOrder.ServiceTasks.Select(st => new ReturnMechanicsTasks
            {
                ServiceTaskId = st.ServiceTaskId.ToString(),
                Name = st.Name,
                LaborCost = st.LaborCost,
                UsedPart = st.UsedParts.Select(up => new UsedPartDTO
                {
                    Quantity = up.Quantity,
                    Part = new PartDTO
                    {
                        PartId = up.Part.PartId,
                        NamePart = up.Part.NamePart,
                        TypePart = up.Part.TypePart,
                        UnitPrice = up.Part.UnitPrice
                    }
                }).FirstOrDefault()
            }).ToList();

            return serviceTasksDto;
        }
    } 
}
