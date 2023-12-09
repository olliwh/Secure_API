using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Secure_API.Models
{
    public class User
    {
        public Guid UserId { get; set; }
        public string? Username { get; set; }
        public string? Name { get; set; }
        public string? Password { get; set; }
        public string? Email { get; set; }
        public string? ImgURL { get; set; }
        public string? CreditCardInformation { get; set; }
        public string? Role { get; set; }

        public void ValidataPassword()
        {
            if (Password == null) throw new ArgumentNullException("password cannot be null");
            if (Password.Length < 8) throw new ArgumentOutOfRangeException("password too short");
            if (Password.Length > 64) throw new ArgumentOutOfRangeException("password too long");
            bool hasUppercaseLetter = Password.Any(char.IsUpper);
            if (!hasUppercaseLetter) throw new ArgumentException("Must contain uppercase");
            bool hasLowercaseLetter = Password.Any(char.IsLower);
            if (!hasLowercaseLetter) throw new ArgumentException("Must contain lowercase");
            bool hasDigit = Password.Any(char.IsDigit);
            if (!hasDigit) throw new ArgumentException("Must contain number");
            bool hasspecialChar = Regex.IsMatch(Password, @"[^A-Za-z0-9]");
            if (!hasspecialChar) throw new ArgumentException("Must contain special character");

        }
    }
}
