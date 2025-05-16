namespace CarWorkshopProjekt.Data
{
    public class UsedPart
    {
        public int UsedPartId { get; set; }
        public int ServiceTaskId { get; set; }
        public int PartId {  get; set; }
        public int Quantity { get; set; }

        // pojedyncza część, której dotyczy ten wpis
        public Part Part { get; set; }

        // nawigacja do ServiceTask
        public ServiceTask ServiceTask { get; set; }
    }
}
