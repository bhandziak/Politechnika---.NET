namespace CarWorkshopProjekt.Mappers
{

    using Riok.Mapperly.Abstractions;
    using CarWorkshopProjekt.DTOs;
    using CarWorkshopProjekt.Data;
    [Mapper]
    public partial class ServiceTaskMapper
    {
        //[HttpPost("addServiceTask")]
        public ServiceTask MapToEntity(AddServiceTask taskDto)
        {
            return new ServiceTask
            {
                ServiceOrderId = Guid.Parse(taskDto.ServiceOrderId),
                Name = taskDto.Name,
                LaborCost = decimal.Parse(taskDto.LaborCost) //konwersja na decimal
            };
        }
    }
}
