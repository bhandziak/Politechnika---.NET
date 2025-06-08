namespace CarWorkshopProjekt.DTOs
{
    public class AddServiceOrder
    {
        public Guid ServiceOrderId { get; set; }
        public Guid VehicleId { get; set; }
        public Guid CustomerId { get; set; }
        public string UserId { get; set; }
        public string StatusOrder { get; set; }
        public string Description { get; set; }
        public DateTime DateFinished { get; set; }

    }
}
