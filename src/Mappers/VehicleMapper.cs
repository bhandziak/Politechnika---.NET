namespace CarWorkshopProjekt.Mappers
{
    using CarWorkshopProjekt.Data;
    using CarWorkshopProjekt.DTOs;
    using Riok.Mapperly.Abstractions;
    [Mapper]
    public partial class VehicleMapper
    {
        public partial Vehicle MapToEntity(AddVehicle dto);

        //[HttpPut("update")]
        public partial void UpdateVehicle(UpdateVehicle vehicleDto, [MappingTarget] Vehicle vehicleEntity);
    }
}
