using Secure_API.Models;

namespace Secure_API.Repositories
{
    public interface ILoginRepository
    {
        User CreateUser(User user);
        User? Login(UserCredentials userCreds);
    }
}