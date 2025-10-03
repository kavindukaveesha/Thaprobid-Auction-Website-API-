using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.Security.Cryptography;
namespace api.Models.security
{

    public class PasswordHasher
    {
        // Generate a random salt
        public static byte[] GenerateSalt()
        {
            using (var rng = RandomNumberGenerator.Create())
            {
                var salt = new byte[16];
                rng.GetBytes(salt);
                return salt;
            }
        }

        // Hash the password using PBKDF2 algorithm
        public static byte[] HashPassword(string password, byte[] salt)
        {
            using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000)) // 10,000 iterations for security
            {
                return pbkdf2.GetBytes(32); // Generate 32-byte hash
            }
        }

        // Verify a password against a stored hash
        public static bool VerifyPassword(string password, byte[] storedHash, byte[] storedSalt)
        {
            var newHash = HashPassword(password, storedSalt);
            return newHash.SequenceEqual(storedHash);
        }
    }
}
