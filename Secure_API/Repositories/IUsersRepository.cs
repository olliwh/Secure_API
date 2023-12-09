using Secure_API.Models;

namespace Secure_API.Repositories
{
    public interface IUsersRepository
    {
        User? GetById(Guid id);
        User Update(User user);
    }
}