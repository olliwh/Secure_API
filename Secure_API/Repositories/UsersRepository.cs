using Microsoft.IdentityModel.Tokens;
using Secure_API.Context;
using Secure_API.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Secure_API.Repositories
{
    public class UsersRepository : IUsersRepository
    {
        private UserDBContext _context;

        public UsersRepository(UserDBContext context)
        {
            _context = context;
        }

        public User? GetById(Guid id)
        {
            return int.Parse(this.User.Claims.First(i => i.Type == "UserId").Value);    
            List<User> users = _context.Users.ToList();
            User? user = users.Find(x => x.UserId == id);
            return AbstractRepository.UserReturn(user);
        }

        public User Update(User user)
        {
            throw new NotImplementedException();
        }

    }
}
