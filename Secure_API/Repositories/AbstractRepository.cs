using Secure_API.Models;

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
                Role = user.Role

            };
        }
    }
}
