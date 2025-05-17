using System.ComponentModel.DataAnnotations;

namespace CarWorkshopProjekt.Data
{
    public class User
    {
        public Guid UserId { get; set; }

        [Required]
        public string Login { get; set; }

        [Required]
        public string Password { get; set; }

        public string Role { get; set; }



        // kolekcja ServiceOrders powiązanych z tym użytkownikiem (User)
        public ICollection<ServiceOrder> ServiceOrders { get; set; } = new List<ServiceOrder>();
        // kolekcja komentarzy napisanych przez użytkownika(User)
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();

    }
}
