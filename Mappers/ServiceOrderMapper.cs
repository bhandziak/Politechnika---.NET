namespace CarWorkshopProjekt.Mappers
{
    using Riok.Mapperly.Abstractions;
    using CarWorkshopProjekt.DTOs;
    using CarWorkshopProjekt.Data;
    [Mapper]
    public partial class ServiceOrderMapper
    {
        public partial List<AddServiceOrder> ToReturnDtoList(List<ServiceOrder> serviceOrders);
        public partial void UpdateServiceOrder(CreateServiceOrder dto, [MappingTarget] ServiceOrder entity);
    }
}
