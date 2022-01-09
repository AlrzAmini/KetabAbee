using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KetabAbee.Domain.Models.User;

namespace KetabAbee.Application.Interfaces.Permission
{
    public interface IPermissionService
    {
        IEnumerable<Role> GetRoles();

        void AddRolesToUser(List<int> selectedRoles, int userId);

        void EditUserRoles(List<int> selectedRoles, int userId);
    }
}
