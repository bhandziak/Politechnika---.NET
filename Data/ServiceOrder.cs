namespace CarWorkshopProjekt.Data
{
    public class ServiceOrder
    {
        public int ServiceOrderId { get; set; }
        public int VehicleId { get; set; }
        public int CustomerId { get; set; }
        public int UserId { get; set; }
        public string StatusOrder { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateFinished { get; set; }
    }
}
