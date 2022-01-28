using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KetabAbee.Data.Context;
using KetabAbee.Domain.Interfaces;
using KetabAbee.Domain.Models.User;
using Microsoft.EntityFrameworkCore;

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

        public User IsUserRegistered(string email, string password)
        {
            return _context.Users.SingleOrDefault(u => u.Email == email && u.Password == password);
        }

        //public bool ActiveAccountByEmail(string emailActiveCode)
        //{
        //    var user = _context.Users.SingleOrDefault(s => s.EmailActivationCode == emailActiveCode);

        //    // user not found
        //    if (user == null)
        //    {
        //        return false;
        //    }

        //    // user is already active
        //    if (user.IsEmailActive)
        //    {
        //        return false;
        //    }

        //    user.IsEmailActive = true;
        //    user.EmailActivationCode = new Random().Next(10000, 99999).ToString();
        //    _context.SaveChanges();

        //    return true;
        //}

        public User GetUserByEmail(string email)
        {
            return _context.Users.SingleOrDefault(u => u.Email == email);
        }

        public User GetUserByEmailActivationCode(string activeCode)
        {
            return _context.Users.SingleOrDefault(u => u.EmailActivationCode == activeCode);
        }

        public bool UpdateUser(User user)
        {
            _context.Users.Update(user);
            _context.SaveChanges();

            return true;
        }

        public async Task<string> GetMobileByUserEmail(string email)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Email == email);

            return user.Mobile;
        }

        public User GetUserByUserName(string userName)
        {
            return _context.Users.SingleOrDefault(u => u.UserName == userName);
        }

        public bool IsOldPasswordCorrect(string username, string oldPass) 
        {
            return _context.Users.Any(u => u.UserName == username && u.Password == oldPass);
        }

        public User GetUserById(int userId)
        {
            return _context.Users.Find(userId);
        }

        public string GetAvatarNameByUserId(int userId)
        {
            try
            {
                return _context.Users.SingleOrDefault(u => u.UserId == userId).AvatarName;
            }
            catch
            {
                return "Error";
            }
        }

        public User GetUserByEmailActive5ThCode(string emailActiveCode)
        {
            return _context.Users.FirstOrDefault(u => u.EmailActivationCode == emailActiveCode);
        }

        public IEnumerable<User> GetUsersForEditAdmin()
        {
            return _context.Users
                .Include(u => u.UserRoles);
        }

        public string GetUserNameByUserId(int userId)
        {
            return _context.Users.SingleOrDefault(u => u.UserId == userId).UserName;
        }

        public string GetEmailByUserId(int userId)
        {
            return _context.Users.SingleOrDefault(u => u.UserId == userId).Email;
        }

        public string GetMobileByUserId(int userId)
        {
            return _context.Users.SingleOrDefault(u => u.UserId == userId).Mobile;
        }

        public List<int> GetUserFavBookIds(int userId)
        {
            return _context.FavoriteBooks.Where(f => f.UserId == userId)
                .Select(f => f.BookId).ToList();
        }

        public int GetUserIdByUserName(string userName)
        {
            return _context.Users.SingleOrDefault(u => u.UserName == userName).UserId;
        }
    }
}
