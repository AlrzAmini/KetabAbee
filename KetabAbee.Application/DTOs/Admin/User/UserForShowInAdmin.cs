using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KetabAbee.Application.DTOs.Admin.User
{
    public class UserForShowInUserListAdminViewModel
    {
        public int Userid { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public DateTime RegisterDate { get; set; }

        public string ImageName { get; set; }

        public string Mobile { get; set; }

        public bool IsActiveByEmail { get; set; }

    }
}
