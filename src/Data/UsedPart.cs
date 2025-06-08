namespace CarWorkshopProjekt.Data
{
    public class UsedPart
    {
        public Guid UsedPartId { get; set; }
        public Guid ServiceTaskId { get; set; }
        public Guid PartId {  get; set; }
        public int Quantity { get; set; }

        // pojedyncza część, której dotyczy ten wpis
        public Part Part { get; set; }

        // nawigacja do ServiceTask
        public ServiceTask ServiceTask { get; set; }
    }
}
