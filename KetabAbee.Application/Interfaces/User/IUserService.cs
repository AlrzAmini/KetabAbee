using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KetabAbee.Application.DTOs;

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

        Task<bool> UpdateUser(Domain.Models.User.User user);

        #endregion
    }
}
