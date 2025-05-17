namespace CarWorkshopProjekt.DTOs
{
    public class ReturnUser
    {
        public Guid UserId { get; set; }
        public string Login { get; set; }
        public string Role { get; set; }
        //bez info o komentarzach i zamowieniach
    }
}
