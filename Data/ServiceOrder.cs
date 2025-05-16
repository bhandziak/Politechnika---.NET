namespace CarWorkshopProjekt.Data
{
    public class ServiceOrder
    {
        public int ServiceOrderId { get; set; }
        public int VehicleId { get; set; }
        public int CustomerId { get; set; }
        public int UserId { get; set; }
        public string StatusOrder { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateFinished { get; set; }

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
