using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KetabAbee.Domain.Models.User;

namespace KetabAbee.Application.DTOs
{
    public class ListUsersViewModel
    {
        public IEnumerable<User> Users { get; set; }
    }

    public class UserViewModel
    {
        public User User { get; set; }
    }
}
