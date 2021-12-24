using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KetabAbee.Application.DTOs;
using KetabAbee.Application.DTOs.Admin.User;

namespace KetabAbee.Application.Interfaces.User
{
    public interface IUserService
    {
        ListUsersViewModel GetUsers();

        #region Account

        Domain.Models.User.User RegisterUser(RegisterViewModel user);

        bool IsEmailExist(string email);

        bool IsUserNameExist(string userName);

        bool IsMobileExist(string mobile);

        Domain.Models.User.User GetUserForLogin(LoginViewModel login);

        bool ActiveAccountByEmail(string emailActiveCode);

        Domain.Models.User.User GetUserByEmail(string email);

        Domain.Models.User.User GetUserByEmailActivationCode(string emailActiveCode);

        bool UpdateUser(Domain.Models.User.User user);

        #endregion

        #region User Panel

        UserInformationInDashboardViewmodel GetInfoByUserEmail(string email);

        UserPanelSideBarViewmodel GetUserPanelSideBarInfoByEmail(string userName);

        UserPanelEditInfoViewModel GetInfoForEditInUserPanel(string userName);

        bool EditUserProfile(UserPanelEditInfoViewModel edit);

        bool IsOldPasswordCorrect(string username, string oldPass);

        bool ChangePasswordInUserPanel(string username, string newPass);

        #endregion

        #region Admin Panel

        IEnumerable<UserForShowInUserListAdminViewModel> GetAllUsersForAdmin();

        FilterUsersViewModel GetAllFilteredUsersInAdmin(FilterUsersViewModel filter);

        bool DeleteUserById(int userId);

        string GetAvatarNameByUserId(int userId);

        #endregion
    }
}
