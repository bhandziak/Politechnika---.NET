namespace CarWorkshopProjekt.Mappers
{
    using Riok.Mapperly.Abstractions;
    using CarWorkshopProjekt.DTOs;
    using CarWorkshopProjekt.Data;
    [Mapper]
    public partial class ServiceOrderMapper
    {
        public static ReturnServiceOrder ToReturnDto(ServiceOrder so)
        {
            return new ReturnServiceOrder
            {
                ServiceOrderId = so.ServiceOrderId,
                StatusOrder = so.StatusOrder,
                Description = so.Description,
                DateFinished = so.DateFinished,
                Customer = new ReturnCustomer
                {
                    CustomerId = so.Customer.CustomerId,
                    NameCustomer = so.Customer.NameCustomer,
                    SurnameCustomer = so.Customer.SurnameCustomer
                },
                Vehicle = new ReturnVehicle
                {
                    VehicleId = so.Vehicle.VehicleId,
                    BrandVehicle = so.Vehicle.BrandVehicle,
                    ModelVehicle = so.Vehicle.ModelVehicle,
                    VINVehicle = so.Vehicle.VINVehicle,
                    RegistralNumberVehicle = so.Vehicle.RegistralNumberVehicle,
                    YearVehicle = so.Vehicle.YearVehicle,
                    ImageURL = so.Vehicle.ImageURL
                },
                Mechanic = so.User == null ? null: new ReturnUser
                {
                    Id = so.User.Id,
                    UserName = so.User.UserName
                }
            };
        }

        public static List<ReturnServiceOrder> ToReturnDtoList(IEnumerable<ServiceOrder> serviceOrders)
        {
            return serviceOrders.Select(ToReturnDto).ToList();
        }
        public partial void UpdateServiceOrder(CreateServiceOrder dto, [MappingTarget] ServiceOrder entity);
    }
}
