using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KetabAbee.Application.DTOs.Admin.Home
{
    public class UsersStatisticsViewModel
    {
        public int AllUsersCount { get; set; }

        public int ValidUsersCount { get; set; }

        public int OnlineUsersCount { get; set; }

        public int AdminsCount { get; set; }

    }
}
