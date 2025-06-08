namespace CarWorkshopProjekt.DTOs
{
    public class CreateServiceOrder
    {
        public Guid serviceOrderID { get; set; }
        public string Description { get; set; } 
        public string UserId { get; set; }
    }
}
