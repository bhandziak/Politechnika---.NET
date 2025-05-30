namespace CarWorkshopProjekt.Data
{
    public class ServiceOrder
    {
        public Guid ServiceOrderId { get; set; }
        public Guid VehicleId { get; set; }
        public Guid CustomerId { get; set; }
        public string UserId { get; set; }
        public string? StatusOrder { get; set; }

        public string? Description { get; set; } // nowe pole !
        public DateTime? DateFinished { get; set; }

        // kolekcja ServiceTasks powiązanych z tym zadaniem (tym ServiceOrder)
        public ICollection<ServiceTask> ServiceTasks { get; set; }
        // nawigacja do User
        public User User { get; set; }
        // nawigacja do Customer'a
        public Customer Customer { get; set; }
        // nawigacja do pojazdu
        public Vehicle Vehicle { get; set; }
        // kolekcja Comments powiązanych z tym ServiceOrder(zleceniem)
        public ICollection<Comment> Comments { get; set; }

    }
}
