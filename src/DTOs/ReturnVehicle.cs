namespace CarWorkshopProjekt.DTOs
{
    public class ReturnVehicle
    {
        public Guid VehicleId { get; set; }
        public string BrandVehicle { get; set; }
        public string ModelVehicle { get; set; }
        public string VINVehicle { get; set; }
        public string RegistralNumberVehicle { get; set; }
        public int YearVehicle { get; set; }
        public string ImageURL { get; set; }
    }
}
