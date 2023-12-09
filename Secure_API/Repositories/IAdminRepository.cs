using Secure_API.Models;

namespace Secure_API.Repositories
{
    public interface IAdminRepository
    {
        User? AsignRole(Guid id, string role);
        User? Delete(Guid id);
        IEnumerable<User>? GetAll();
    }
}