using Secure_API.Models;

namespace Secure_API.Repositories
{
    public interface IUsersRepository
    {
        User? GetById(Guid id);
        User Update(Guid id, User newData);
        User ChangePassword(Guid id, UserCredentials newData);
    }
}