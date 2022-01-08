using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KetabAbee.Domain.Models.User;

namespace KetabAbee.Domain.Interfaces
{
    public interface IPermissionRepository
    {
        #region Role

        IEnumerable<Role> GetRoles();

        void AddRolesToUser(List<int> selectedRoleIds,int userId);

        #endregion
    }
}
