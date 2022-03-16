using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KetabAbee.Data.Context;
using KetabAbee.Domain.Interfaces;
using KetabAbee.Domain.Models.Comment.ProductComment;
using KetabAbee.Domain.Models.Order;
using KetabAbee.Domain.Models.Products;
using KetabAbee.Domain.Models.Ticket;
using KetabAbee.Domain.Models.User;
using KetabAbee.Domain.Models.Wallet;
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

        public async Task<int> RegisterUser(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return user.UserId;
        }

        public IEnumerable<User> GetUsers()
        {
            return _context.Users
                .Include(u => u.UserRoles)
                .OrderByDescending(u => u.RegisterDate);
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

        public async Task<User> IsUserRegistered(string email, string password)
        {
            return await _context.Users.SingleOrDefaultAsync(u => u.Email == email && u.Password == password);
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

        public async Task<string> GetAvatarNameByUserId(int userId)
        {
            try
            {
                var user = await _context.Users.FindAsync(userId);
                return user.AvatarName;
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
            return _context.Users.FirstOrDefault(u => u.UserId == userId)?.UserName;
        }

        public string GetEmailByUserId(int userId)
        {
            return _context.Users.FirstOrDefault(u => u.UserId == userId)?.Email;
        }

        public string GetMobileByUserId(int userId)
        {
            return _context.Users.SingleOrDefault(u => u.UserId == userId)?.Mobile;
        }

        public async Task<List<int>> GetUserFavBookIds(int userId)
        {
            return await _context.FavoriteBooks.Where(f => f.UserId == userId)
                .Select(f => f.BookId).ToListAsync();
        }

        public int AllUsersCount()
        {
            return _context.Users.Count();
        }

        public int GetUserIdByUserName(string userName)
        {
            return _context.Users.SingleOrDefault(u => u.UserName == userName)!.UserId;
        }

        public string GetUserAddressByUserId(int userId)
        {
            return GetUserById(userId).Address;
        }

        public int GetLastNDaysUsersCount(int n)
        {
            return _context.Users.Count(u => u.RegisterDate > DateTime.Now.AddDays(-n));
        }

        public int ValidUsersCount()
        {
            return _context.Users.Count(u => u.IsEmailActive);
        }

        public int AdminsCount()
        {
            return _context.UserRoles
                .Select(r => r.UserId)
                .Distinct().Count();
        }

        public int OnlineUsersCount()
        {
            return _context.Users.Count(u => u.IsOnline);
        }

        public void AddUserIp(UserIp userIp)
        {
            _context.UserIps.Add(userIp);
            _context.SaveChanges();
        }

        public bool CheckUserIpIsNotRepetitious(UserIp userIp)
        {
            return _context.UserIps.Any(i => i.UserId == userIp.UserId && i.Ip == userIp.Ip);
        }

        public async Task<List<int>> GetUserRoleIds(int userId)
        {
            return await _context.UserRoles
                .Where(r => r.UserId == userId)
                .Select(r => r.RoleId).ToListAsync();
        }

        public async Task<User> GetUserByIdWithIncludes(int userId)
        {
            return await _context.Users
                .Include(u => u.UserRoles)
                .Include(u => u.Tickets)
                .Include(u => u.TicketAnswers)
                .Include(u => u.Wallets)
                .Include(u => u.FavoriteBooks)
                .Include(u => u.Orders)
                .Include(u => u.UserBooks)
                .Include(u => u.BookScores)
                .Include(u => u.ProductComments)
                .Include(u => u.UserIps)
                .FirstOrDefaultAsync(u => u.UserId == userId);
        }

        public List<string> GetUserIps(int userId)
        {
            return _context.UserIps.Where(i => i.UserId == userId)
                .Select(i => i.Ip)
                .ToList();
        }

        public List<BookScore> GetUserBookScores(int userId)
        {
            return _context.BookScores
                .Include(s => s.User)
                .Include(s => s.Book)
                .Where(s => s.UserId == userId)
                .ToList();
        }

        public List<FavoriteBook> GetUserFavoriteBooks(int userId)
        {
            return _context.FavoriteBooks
                .Include(f => f.Book)
                .Include(f => f.User)
                .Where(f => f.UserId == userId && f.IsLiked)
                .ToList();
        }

        public List<Order> GetUserOrders(int userId)
        {
            return _context.Orders
                .Include(o => o.User)
                .Where(o => o.UserId == userId)
                .OrderByDescending(o => o.CreateDate)
                .ToList();
        }

        public List<ProductComment> GetUserProductComments(int userId)
        {
            return _context.ProductComments
                .Include(c => c.Product)
                .Where(c => c.UserId == userId)
                .ToList();
        }

        public List<Book> GetUserBooks(int userId)
        {
            return _context.UserBooks
                .Include(b => b.Book)
                .Where(b => b.UserId == userId)
                .Select(ub => ub.Book)
                .Distinct()
                .ToList();
        }

        public List<Ticket> GetUserTickets(int userId)
        {
            return _context.Tickets
                .Include(t => t.Sender)
                .OrderByDescending(t => t.TicketSendDate)
                .Where(t => t.SenderId == userId)
                .ToList();
        }

        public List<Wallet> GetUserWallets(int userId)
        {
            return _context.Wallets
                .Include(w => w.User)
                .Where(w => w.UserId == userId)
                .ToList();
        }

        public async Task<List<BannedIp>> GetBannedIps()
        {
            return await _context.BannedIps.ToListAsync();
        }

        public void AddIpToBannedIps(int userId, string userIp)
        {
            _context.BannedIps.Add(new BannedIp
            {
                Ip = userIp,
                UserId = userId
            });
            _context.SaveChanges();
        }

        public void RemoveIpFromBannedIps(int userId, string ip)
        {
            _context.BannedIps.Remove(GetBannedIpByInfo(userId, ip));
            _context.SaveChanges();
        }

        public BannedIp GetBannedIpByInfo(int userId, string ip)
        {
            return _context.BannedIps.FirstOrDefault(b => b.UserId == userId && b.Ip == ip);
        }

        public async Task<long> GetUserWalletBalance(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            return user.WalletBalance;
        }

        public async Task<int> GetUserFavBookCount(int userId)
        {
            return await _context.FavoriteBooks.CountAsync(f => f.UserId == userId);
        }

        public void DeleteUserRoleByUserRoleId(int userRoleId)
        {
            var userRole = _context.UserRoles.Find(userRoleId);
            userRole.IsDelete = true;
            _context.SaveChanges();
        }

        public void DeleteUserTickets(List<int> ticketIds)
        {
            foreach (var ticket in ticketIds.Select(id => _context.Tickets.Find(id)))
            {
                ticket.IsDelete = true;
            }

            _context.SaveChanges();
        }

        public void DeleteUserTicketAnswers(List<int> answerIds)
        {
            foreach (var answer in answerIds.Select(id => _context.TicketAnswers.Find(id)))
            {
                answer.IsDelete = true;
            }

            _context.SaveChanges();
        }

        public void DeleteUserWallets(List<int> walletIds)
        {
            foreach (var wallet in walletIds.Select(id => _context.Wallets.Find(id)))
            {
                wallet.IsDelete = true;
            }

            _context.SaveChanges();
        }

        public void DeleteUserFavs(List<int> favIds)
        {
            foreach (var fav in favIds.Select(id => _context.FavoriteBooks.Find(id)))
            {
                fav.IsDelete = true;
            }

            _context.SaveChanges();
        }

        public void DeleteUserOrders(List<int> orderIds)
        {
            foreach (var order in orderIds.Select(id => _context.Orders.Find(id)))
            {
                order.IsDelete = true;
            }

            _context.SaveChanges();
        }

        public void DeleteUserBooks(List<int> userBookIds)
        {
            foreach (var userBook in userBookIds.Select(id => _context.UserBooks.Find(id)))
            {
                userBook.IsDelete = true;
            }

            _context.SaveChanges();
        }

        public void DeleteUserScores(List<int> userScoreIds)
        {
            foreach (var bookScore in userScoreIds.Select(id => _context.BookScores.Find(id)))
            {
                bookScore.IsDelete = true;
            }

            _context.SaveChanges();
        }

        public void DeleteUserComments(List<int> userCommentIds)
        {
            foreach (var comment in userCommentIds.Select(id => _context.ProductComments.Find(id)))
            {
                comment.IsDelete = true;
            }

            _context.SaveChanges();
        }

        public async Task<bool> AddUserIpToBannedIps(string userIp)
        {
            try
            {
                _context.BannedIps.Add(new BannedIp
                {
                    Ip = userIp,
                    UserId = null
                });
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> IsIpExistInBanneds(string ip)
        {
            return await _context.BannedIps.AnyAsync(i => i.Ip == ip);
        }

        public async Task<User> GetUserByIdWithIncludeIps(int userId)
        {
            return await _context.Users
                .Include(u => u.UserIps)
                .FirstOrDefaultAsync(u => u.UserId == userId);
        }

        public IEnumerable<User> GetLastNHoursUsers(int n)
        {
            return _context.Users.Where(u => u.RegisterDate > DateTime.Now.AddHours(-n));
        }
    }
}
