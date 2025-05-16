namespace CarWorkshopProjekt.Data
{
    public class User
    {
        public int UserId { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }


        // kolekcja ServiceOrders powiązanych z tym użytkownikiem (User)
        public ICollection<ServiceOrder> ServiceOrders { get; set; }
        // kolekcja komentarzy napisanych przez użytkownika(User)
        public ICollection<Comment> Comments { get; set; }  

    }
}
