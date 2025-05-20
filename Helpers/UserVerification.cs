using CarWorkshopProjekt.Data;
using Microsoft.EntityFrameworkCore;

namespace CarWorkshopProjekt.Helpers
{
    public class UserVerification
    {

        //weryfikuje czy użytkownik ma prawo dostępu po "role"
        public static bool VerifyUser(Guid userId, AppDbContext _context, string role)
        {
            var user = _context.Users.FirstOrDefault(u => u.UserId == userId);
            if (user == null) return false;

            //logika: tylko osoby o odpowiednim "role" mają uprawnienia
            return user.Role != null && user.Role.Equals(role, StringComparison.OrdinalIgnoreCase);
        }
    }
}
