using System.Security.Cryptography;
using System.Text;

namespace CarWorkshopProjekt.Helpers
{
    public class PasswordHasher
    {
        public static string HashPassword(string password)
        {
            // Generowanie ziarna
            byte[] salt = RandomNumberGenerator.GetBytes(16);

            // Tworzenie hash z PBKDF2 (100 000 iteracji)
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 100_000, HashAlgorithmName.SHA256);
            byte[] hash = pbkdf2.GetBytes(32); // 256-bitowy hash

            // Łączenie soli + hasza
            byte[] hashBytes = new byte[48]; // 16 bajtów ziarna + 32 bajty hasza
            Buffer.BlockCopy(salt, 0, hashBytes, 0, 16);
            Buffer.BlockCopy(hash, 0, hashBytes, 16, 32);

            // Zwracamy jako Base64
            return Convert.ToBase64String(hashBytes);
        }

        public static bool VerifyPassword(string enteredPassword, string storedHash)
        {
            byte[] hashBytes = Convert.FromBase64String(storedHash);

            byte[] salt = new byte[16];
            Buffer.BlockCopy(hashBytes, 0, salt, 0, 16);
            //hashowanie ponowne hasla z tym samym ziarnem co hashowane haslo w bazie
            var pbkdf2 = new Rfc2898DeriveBytes(enteredPassword, salt, 100_000, HashAlgorithmName.SHA256);
            byte[] hash = pbkdf2.GetBytes(32);

            for (int i = 0; i < 32; i++)
            {
                if (hashBytes[i + 16] != hash[i])
                    return false;
            }

            return true;
        }

    }
}
