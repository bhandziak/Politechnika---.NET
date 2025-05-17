using CarWorkshopProjekt.Data;
using Microsoft.EntityFrameworkCore;

namespace CarWorkshopProjekt.Helpers
{
    public class UserVerification
    {

        // Weryfikuje, czy użytkownik ma prawo dostępu po roli usera
        public static bool VerifyUser(Guid userId, AppDbContext _context, string role)
        {
            var user = _context.Users.FirstOrDefault(u => u.UserId == userId);
            if (user == null) return false;

            //logika: tylko admini mogą pobierać listę użytkowników
            return user.Role != null && user.Role.Equals(role, StringComparison.OrdinalIgnoreCase);
        }
    }
}
