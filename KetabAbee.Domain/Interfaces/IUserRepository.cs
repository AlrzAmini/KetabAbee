using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KetabAbee.Domain.Models.Comment.ProductComment;
using KetabAbee.Domain.Models.Order;
using KetabAbee.Domain.Models.Products;
using KetabAbee.Domain.Models.Ticket;
using KetabAbee.Domain.Models.User;
using KetabAbee.Domain.Models.Wallet;

namespace KetabAbee.Domain.Interfaces
{
    public interface IUserRepository
    {
        IEnumerable<User> GetUsers();

        #region Account

        Task<bool> RegisterUser(User user);

        bool IsEmailExist(string email);

        bool IsUserNameExist(string userName);

        bool IsMobileExist(string mobile);

        Task<User> IsUserRegistered(string email, string password);

        //bool ActiveAccountByEmail(string emailActiveCode);

        User GetUserByEmail(string email);

        User GetUserByEmailActivationCode(string activeCode);

        bool UpdateUser(User user);

        User GetUserByEmailActive5ThCode(string emailActiveCode);

        void AddUserIp(UserIp userIp);

        bool CheckUserIpIsNotRepetitious(UserIp userIp);


        #endregion

        #region User Panel

        User GetUserByUserName(string userName);

        bool IsOldPasswordCorrect(string username, string oldPass);

        List<int> GetUserFavBookIds(int userId);

        string GetUserAddressByUserId(int userId);

        #endregion

        #region Admin Panel

        User GetUserById(int userId);

        string GetAvatarNameByUserId(int userId);

        IEnumerable<User> GetUsersForEditAdmin();

        string GetUserNameByUserId(int userId);

        string GetEmailByUserId(int userId);

        string GetMobileByUserId(int userId);

        IEnumerable<User> GetLastNDaysUsers(int n);

        int AllUsersCount();

        int ValidUsersCount();

        int AdminsCount();

        int OnlineUsersCount();

        List<int> GetUserRoleIds(int userId);

        User GetUserByIdWithIncludes(int userId);

        List<string> GetUserIps(int userId);

        List<BookScore> GetUserBookScores(int userId);

        List<FavoriteBook> GetUserFavoriteBooks(int userId);

        List<Order> GetUserOrders(int userId);

        List<ProductComment> GetUserProductComments(int userId);

        List<Book> GetUserBooks(int userId);

        List<Ticket> GetUserTickets(int userId);

        List<Wallet> GetUserWallets(int userId);

        Task<List<BannedIp>> GetBannedIps();

        void AddIpToBannedIps(int userId, string userIp);

        void RemoveIpFromBannedIps(int userId, string ip);

        BannedIp GetBannedIpByInfo(int userId, string ip);

        #endregion

        int GetUserIdByUserName(string userName);
    }
}
