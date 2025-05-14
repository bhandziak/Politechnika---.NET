namespace CarWorkshopProjekt.Data
{
    public class ServiceTask
    {
        public int ServiceTaskId { get; set; }
        public int ServiceOrderId { get; set; }
        public string Description { get; set; }
        public decimal LaborCost { get; set; }
    }
}
