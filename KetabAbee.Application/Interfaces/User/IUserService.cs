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
    }
}
