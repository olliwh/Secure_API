using Microsoft.AspNetCore.Identity;
using System.Security.Cryptography;

namespace Secure_API.Models
{
    public static class PasswordHasher
    {
        public static string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password, 13);
        }
        public static bool VerifyPassword(string password, string passwordHash)
        {
            return BCrypt.Net.BCrypt.Verify(password, passwordHash);

        }
    }
}
