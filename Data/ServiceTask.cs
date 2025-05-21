namespace CarWorkshopProjekt.Data
{
    public class ServiceTask
    {
        public Guid ServiceTaskId { get; set; }
        public Guid ServiceOrderId { get; set; }
        public string Name { get; set; } // was Description
        public decimal LaborCost { get; set; }

        // kolekcja użytych części powiązanych z tym zadaniem
        public ICollection<UsedPart> UsedParts { get; set; }
        // nawigacja do ServiceOrder 
        public ServiceOrder ServiceOrder { get; set; } 
    }
}
