using CarWorkshopProjekt.Data;
using Microsoft.EntityFrameworkCore;

namespace CarWorkshopProjekt.Helpers
{
    public class UserVerification
    {

        //weryfikuje czy użytkownik ma prawo dostępu po "role"
        public static bool VerifyUser(Guid userId, AppDbContext _context, string[] role)
        {
            var user = _context.Users.FirstOrDefault(u => u.UserId == userId);
            if (user == null || user.Role == null) return false;

            //logika: tylko osoby o odpowiednim "role" mają uprawnienia
            //sprawdzenie czy rola użytkownika znajduje się w tablicy 'role' (ignoruje wielkość liter)
            return role.Any(r => string.Equals(r, user.Role, StringComparison.OrdinalIgnoreCase));
        }
    }
}
