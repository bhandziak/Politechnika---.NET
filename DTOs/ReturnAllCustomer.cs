using CarWorkshopProjekt.Data;

namespace CarWorkshopProjekt.DTOs
{
    public class ReturnAllCustomer
    {
        public Guid CustomerId { get; set; }
        public string NameCustomer { get; set; }
        public string SurnameCustomer { get; set; }
        public string PhoneNumber { get; set; }

        // kolekcja ServiceOrders powiązanych z tym customerem
        public ICollection<ServiceOrder> ServiceOrders { get; set; }

    }
}
