namespace CarWorkshopProjekt.Data
{
    public class UsedPart
    {
        public int UsedPartId { get; set; }
        public int ServiceTaskId { get; set; }
        public int PartId {  get; set; }
        public int Quantity { get; set; }
    }
}
