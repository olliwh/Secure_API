using Microsoft.EntityFrameworkCore;
using Secure_API.Models;
using System.Security.Authentication;

namespace Secure_API.Repositories
{
    public class UsersRepo : IUsersRepository, IAdminRepository, ILoginRepository
    {
        private Guid _nextId;
        public List<User> _users;
        public UsersRepo()
        {
            _users = new List<User>()
            {
                new User() {UserId = Guid.NewGuid(), Username = "UserB", Name = "Bob", CreditCardInformation = "1234 123", Email = "bob@gmail.com", ImgURL = null, Password = "Pass123!", Role = "User"},
                new User() {UserId = Guid.NewGuid(), Username = "UserA", Name = "Alice", CreditCardInformation = "1234 123", Email = "alice@gmail.com", ImgURL = null, Password = "Pass123!", Role = "User"},
                new User() {UserId = Guid.NewGuid(), Username = "Admin", Name = "Admin", CreditCardInformation = "1234 123", Email = "admin@gmail.com", ImgURL = null, Password = "Pass123!", Role = "Admin"},
            };

        }

        public User? GetById(Guid id)
        {
            User? user = _users.FirstOrDefault(x => x.UserId == id);
            if (user == null) return null;
            return user;
        }

        public User Update(Guid id, User newData)
        {
            User? user = _users.FirstOrDefault(x => x.UserId == id);
            if (user == null) return null;
            user.Username = newData.Username;
            user.Email = newData.Email;
            user.CreditCardInformation = newData.CreditCardInformation;
            user.ImgURL = newData.ImgURL;
            user.Name = newData.Name;

            return AbstractRepository.UserReturn(user);
        }

        public User ChangePassword(Guid id, UserCredentials newData)
        {
            User? user = _users.FirstOrDefault(x => x.UserId == id);

            if (user != null)
            {
                bool correctPass = PasswordHasher.VerifyPassword(newData.Password, user.Password);
                if (!correctPass) throw new InvalidCredentialException("Invalid Credentials");
                if (newData.NewPassword == newData.NewPassword2)
                {
                    user.Password = AbstractRepository.ValidatePassword(newData.NewPassword);
                    return AbstractRepository.UserReturn(user);
                }
                throw new Exception("New passwords do not match");

            }
            return null;
        }
        //ADMIN

        public User? AsignRole(Guid id, string role)
        {
            User? userToUpdate = _users.FirstOrDefault(user => user.UserId == id);
            if (userToUpdate == null) return null;
            userToUpdate.Role = role;
            return UserReturnAdmin(userToUpdate);
        }

        public User? Delete(Guid id)
        {
            User? userToDelete = _users.FirstOrDefault(user => user.UserId == id);
            if (userToDelete == null) return null;
            _users.Remove(userToDelete);
            return UserReturnAdmin(userToDelete);
        }

        public IEnumerable<User>? GetAll()
        {
            List<User> users = _users.ToList();
            if (users == null) return null;
            return users.Select(UserReturnAdmin);
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

        //Login
        public User CreateUser(User user)
        {
            bool usernameExists = _users.Any(x => x.Username == user.Username);
            if (usernameExists) throw new ArgumentException("Username already exist");
            user.Password = AbstractRepository.ValidatePassword(user.Password);
            string a = user.Password;
            user.UserId = new Guid();
            user.Role = "User";
            _users.Add(user);
            return AbstractRepository.UserReturn(user);
        }

        public User? Login(UserCredentials userCreds)
        {
            List<User> users = _users.ToList();
            User? found = users.Find(x => x.Username == userCreds.Username);
            if (found != null)
            {
                bool correctPass = PasswordHasher.VerifyPassword(userCreds.Password, found.Password);
                if (correctPass) return AbstractRepository.UserReturn(found);
            }
            return null;
        }
    }
}
