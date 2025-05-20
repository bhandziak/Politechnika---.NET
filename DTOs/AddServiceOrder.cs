namespace CarWorkshopProjekt.DTOs
{
    public class AddServiceOrder
    {
        public Guid ServiceOrderId { get; set; }
        public Guid VehicleId { get; set; }
        public Guid CustomerId { get; set; }
        public Guid UserId { get; set; }
        public string StatusOrder { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateFinished { get; set; }

    }
}
