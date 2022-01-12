using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KetabAbee.Application.DTOs.Admin.Role
{
    public class AddRoleViewModel
    {
        public Domain.Models.User.Role Role { get; set; }

        public List<int> SelectedPermissions { get; set; }
    }
}
