using Microsoft.EntityFrameworkCore;
using Secure_API.Context;
using Secure_API.Models;

namespace Secure_API.Repositories
{
    public class AdminRepository : IAdminRepository
    {
        private UserDBContext _context;
        public AdminRepository(UserDBContext context)
        {
            _context = context;
        }
        public IEnumerable<User>? GetAll()
        {
            List<User> users = _context.Users.ToList();
            if (users == null) return null;
            return users.Select(UserReturnAdmin);
        }
        public User? Delete(Guid id)
        {
            User? userToDelete = _context.Users.FirstOrDefault(user => user.UserId == id);
            if (userToDelete == null) return null;
            _context.Users.Remove(userToDelete);
            _context.SaveChanges();
            return UserReturnAdmin(userToDelete);
        }
        public User? AsignRole(Guid id, string role)
        {
            User? userToUpdate = _context.Users.FirstOrDefault(user => user.UserId == id);
            if (userToUpdate == null) return null;
            userToUpdate.Role = role;
            _context.SaveChanges();
            return UserReturnAdmin(userToUpdate);
        }
        private User UserReturnAdmin(User user)
        {
            return new User
            {
                UserId = user.UserId,
                Username = user.Username,
                Name = user.Name,
                Email = user.Email,
                ImgURL = user.ImgURL,
                Role = user.Role
            };
        }
    }
}
