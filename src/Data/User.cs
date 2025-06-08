using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace CarWorkshopProjekt.Data
{
    public class User : IdentityUser
    {
        // kolekcja ServiceOrders powiązanych z tym użytkownikiem (User)
        public ICollection<ServiceOrder> ServiceOrders { get; set; } = new List<ServiceOrder>();
        // kolekcja komentarzy napisanych przez użytkownika(User)
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();

    }
}
