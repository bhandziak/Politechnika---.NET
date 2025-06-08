namespace CarWorkshopProjekt.DTOs
{
    public class UpdateCustomer
    {
        public Guid CustomerId { get; set; }
        public string NameCustomer { get; set; }
        public string SurnameCustomer { get; set; }
        public string PhoneNumber { get; set; }
    }
}
