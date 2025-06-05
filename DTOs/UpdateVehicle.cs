namespace CarWorkshopProjekt.DTOs
{
    public class UpdateVehicle
    {
        public Guid VehicleId { get; set; }
        public string BrandVehicle { get; set; }
        public string ModelVehicle { get; set; }
        public string VINVehicle { get; set; }
        public string RegistralNumberVehicle { get; set; }
        public int YearVehicle { get; set; }

    }
}
