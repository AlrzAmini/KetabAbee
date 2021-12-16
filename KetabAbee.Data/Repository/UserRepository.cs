using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KetabAbee.Data.Context;
using KetabAbee.Domain.Interfaces;
using KetabAbee.Domain.Models.User;

namespace KetabAbee.Data.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly KetabAbeeDBContext _context;

        public UserRepository(KetabAbeeDBContext context)
        {
            _context = context;
        }

        public bool RegisterUser(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();

            return true;
        }

        public IEnumerable<User> GetUsers()
        {
            return _context.Users;
        }

        public bool IsEmailExist(string email)
        {
            return _context.Users.Any(u => u.Email == email);
        }

        public bool IsUserNameExist(string userName)
        {
            return _context.Users.Any(u => u.UserName == userName);
        }

        public bool IsMobileExist(string mobile)
        {
            return _context.Users.Any(u => u.Mobile == mobile.Trim());
        }

        public User IsUserRegistered(string email,string password)
        {
            return _context.Users.SingleOrDefault(u=>u.Email == email && u.Password == password);
        }
    }
}
