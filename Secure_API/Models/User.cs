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
    }
}
