using Secure_API.Models;
using System.Text.RegularExpressions;

namespace Secure_API.Repositories
{
    public class AbstractRepository
    {
        public static User UserReturn(User user)
        {
            return new User
            {
                UserId = user.UserId,
                Username = user.Username,
                Name = user.Name,
                Email = user.Email,
                ImgURL = user.ImgURL,
                CreditCardInformation = user.CreditCardInformation,
                Role = user.Role,
            };
        }

        public static void ValidatePassword(string? password)
        {
            if (password == null) throw new ArgumentNullException("password cannot be null");
            if (password.Length < 8) throw new ArgumentOutOfRangeException("password too short");
            if (password.Length > 64) throw new ArgumentOutOfRangeException("password too long");
            bool hasUppercaseLetter = password.Any(char.IsUpper);
            if (!hasUppercaseLetter) throw new ArgumentException("Must contain uppercase");
            bool hasLowercaseLetter = password.Any(char.IsLower);
            if (!hasLowercaseLetter) throw new ArgumentException("Must contain lowercase");
            bool hasDigit = password.Any(char.IsDigit);
            if (!hasDigit) throw new ArgumentException("Must contain number");
            bool hasspecialChar = Regex.IsMatch(password, @"[^A-Za-z0-9]");
            if (!hasspecialChar) throw new ArgumentException("Must contain special character");
        }
    }
}
