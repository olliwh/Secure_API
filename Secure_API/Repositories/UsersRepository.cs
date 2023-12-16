using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Secure_API.Context;
using Secure_API.Models;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Authentication;
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
            User user = _context.Users.FirstOrDefault(x => x.UserId == id);
            if (user == null) return null;
            return AbstractRepository.UserReturn(user);
        }

        public User Update(Guid id, User newData)
        {
            User? user = _context.Users.FirstOrDefault(x => x.UserId == id);
            if (user == null) return null;
            user.Username = newData.Username;
            user.Email = newData.Email;
            user.CreditCardInformation = newData.CreditCardInformation;
            user.ImgURL = newData.ImgURL;
            user.Name = newData.Name;
            _context.Entry(user).State = EntityState.Modified;
            _context.SaveChanges();
            return AbstractRepository.UserReturn(user);
        }
        public User ChangePassword(Guid id, UserCredentials newData)
        {
            User? user = _context.Users.FirstOrDefault(x => x.UserId == id);

            if (user != null)
            {
                bool correctPass = PasswordHasher.VerifyPassword(newData.Password, user.Password);
                if (!correctPass) throw new InvalidCredentialException("Invalid Credentials");
                if (newData.NewPassword == newData.NewPassword2)
                {
                    user.Password = AbstractRepository.ValidatePassword(newData.NewPassword);
                    _context.SaveChanges();
                    return AbstractRepository.UserReturn(user);
                }
                throw new Exception("New passwords do not match");

            }
            return null;
        }
    }
}
