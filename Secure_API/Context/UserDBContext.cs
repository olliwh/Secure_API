using Microsoft.EntityFrameworkCore;
using Secure_API.Models;

namespace Secure_API.Context
{
    public class UserDBContext : DbContext
    {
        public UserDBContext(DbContextOptions<UserDBContext> options)
            : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
    }
}
