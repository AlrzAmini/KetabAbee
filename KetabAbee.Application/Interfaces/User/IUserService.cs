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

        #region Register

        int AddUser(Domain.Models.User.User user);

        bool IsEmailExist(string email);

        bool IsUserNameExist(string userName);

        #endregion
    }
}
