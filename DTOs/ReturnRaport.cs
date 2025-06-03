namespace CarWorkshopProjekt.DTOs
{
    public class ReturnRaport
    {
        public Guid ServiceOrderId { get; set; }
        public string? StatusOrder { get; set; }
        public string? Description { get; set; }
        public DateTime? DateFinished { get; set; }
        public ReturnCustomer Customer { get; set; }
        public ReturnVehicle Vehicle { get; set; }
        public ReturnUser Mechanic { get; set; }
        public List<AddServiceTask> ServiceTasks { get; set; }
    }
}
