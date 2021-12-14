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

        int AddUser(User user);

        bool IsEmailExist(string email);

        bool IsUserNameExist(string userName);
    }
}
