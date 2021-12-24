﻿using System;
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

        bool ActiveAccountByEmail(string emailActiveCode);

        User GetUserByEmail(string email);

        User GetUserByEmailActivationCode(string activeCode);

        bool UpdateUser(User user);


        #endregion

        #region User Panel

        User GetUserByUserName(string userName);

        bool IsOldPasswordCorrect(string username, string oldPass);

        #endregion

        #region Admin Panel

        User GetUserById(int userId);

        string GetAvatarNameByUserId(int userId);

        #endregion


    }
}
