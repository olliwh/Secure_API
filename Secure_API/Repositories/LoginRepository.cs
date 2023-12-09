using Microsoft.EntityFrameworkCore;
using Secure_API.Context;
using Secure_API.Models;

namespace Secure_API.Repositories
{
    public class LoginRepository : ILoginRepository
    {
        private UserDBContext _context;
        public LoginRepository(UserDBContext context)
        {
            _context = context;
        }
        public User CreateUser(User user)
        {
            bool usernameExists = _context.Users.Any(x => x.Username == user.Username);
            if (usernameExists) throw new ArgumentException("Username already exist");
            user.ValidataPassword();
            user.UserId = new Guid();
            user.Role = "User";
            _context.Users.Add(user);
            _context.SaveChanges();
            return AbstractRepository.UserReturn(user);
        }
        public User? Login(UserCredentials userCreds)
        {
            List<User> users = _context.Users.ToList();

            User? found = users.Find(x => x.Username == userCreds.Username);
            if (found != null && found.Password == userCreds.Password) return AbstractRepository.UserReturn(found);
            return null;
        }
    }
}
