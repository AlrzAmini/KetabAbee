﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KetabAbee.Application.DTOs;
using KetabAbee.Application.DTOs.Admin.Home;
using KetabAbee.Application.DTOs.Admin.User;
using KetabAbee.Application.DTOs.Admin.Wallet;
using KetabAbee.Application.DTOs.Book;
using KetabAbee.Domain.Models.User;
using Microsoft.AspNetCore.Http;

namespace KetabAbee.Application.Interfaces.User
{
    public interface IUserService
    {
        ListUsersViewModel GetUsers();

        #region Account

        Task<Domain.Models.User.User> RegisterUser(RegisterViewModel user);

        bool IsEmailExist(string email);

        bool IsUserNameExist(string userName);

        bool IsMobileExist(string mobile);

        Task<Domain.Models.User.User> GetUserForLogin(LoginViewModel login);

        //bool ActiveAccountByEmail(string emailActiveCode);

        Domain.Models.User.User GetUserByEmail(string email);

        Domain.Models.User.User GetUserByEmailActivationCode(string emailActiveCode);

        bool UpdateUser(Domain.Models.User.User user);

        bool EmailActivatorBy5ThCode(string activateCode);

        void AddUserIp(UserIp userIp);

        #endregion

        #region User Panel

        UserInformationInDashboardViewmodel GetInfoByUserEmail(string email);

        UserPanelSideBarViewmodel GetUserPanelSideBarInfoByEmail(string userName);

        UserPanelEditInfoViewModel GetInfoForEditInUserPanel(string userName);

        bool EditUserProfile(UserPanelEditInfoViewModel edit);

        bool IsOldPasswordCorrect(string username, string oldPass);

        bool ChangePasswordInUserPanel(string username, string newPass);

        List<int> GetUserFavBookIds(int userId);

        string GetUserAddressByUserId(int userId);

        #endregion

        #region Admin Panel

        IEnumerable<UserForShowInUserListAdminViewModel> GetAllUsersForAdmin();

        FilterUsersViewModel GetAllFilteredUsersInAdmin(FilterUsersViewModel filter);

        bool DeleteUserById(int userId);

        string GetAvatarNameByUserId(int userId);

        int AddUser(AddUserFromAdminViewModel user, IFormFile imgFile);

        EditUserViewModel GetUserForEditInAdmin(int userId);

        string GetUserNameByUserId(int userId);

        string GetEmailByUserId(int userId);

        string GetMobileByUserId(int userId);

        bool EditUserFromAdmin(EditUserViewModel user);

        ChargeWalletFromAdminViewModel GetChargeInfoForAdmin(int userId);

        IEnumerable<Domain.Models.User.User> GetLastNDaysUsers(int n);

        IEnumerable<UserForAutoCompleteViewModel> GetUsersForAutoComplete();

        UsersStatisticsViewModel GetUsersStaticsForAdmin();

        List<int> GetUserRoleIds(int userId);

        #endregion
    }
}
