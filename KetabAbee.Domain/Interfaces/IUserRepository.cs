using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KetabAbee.Domain.Models.User;

namespace KetabAbee.Domain.Interfaces
{
    public interface IUserRepository
    {
        IEnumerable<User> GetUsers();

        #region Account

        bool RegisterUser(User user);

        bool IsEmailExist(string email);

        bool IsUserNameExist(string userName);

        bool IsMobileExist(string mobile);

        User IsUserRegistered(string email, string password);

        //bool ActiveAccountByEmail(string emailActiveCode);

        User GetUserByEmail(string email);

        User GetUserByEmailActivationCode(string activeCode);

        bool UpdateUser(User user);

        User GetUserByEmailActive5ThCode(string emailActiveCode);


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

        #endregion

        int GetUserIdByUserName(string userName);

    }
}
