using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KetabAbee.Domain.Models.Permission;
using KetabAbee.Domain.Models.User;

namespace KetabAbee.Domain.Interfaces
{
    public interface IPermissionRepository
    {
        #region Role

        IEnumerable<Role> GetRoles();

        void AddRolesToUser(List<int> selectedRoleIds, int userId);

        void EditUserRoles(List<int> selectedRoleIds, int userId);

        Role GetRoleById(int roleId);

        bool DeleteRole(int roleId);

        bool UpdateRole(Role role);

        int AddRole(Role role);

        #endregion

        #region Permission

        IEnumerable<Permission> GetPermissions();

        void AddPermissionsToRole(int roleId, List<int> selectedPermissions);

        List<int> GetPermissionIdsOfRoleByRoleId(int roleId);

        void UpdatePermissionOfRole(int roleId, List<int> selectedPermission);

        List<int> GetUserRolesByUserId(int userId);

        List<int> GetRolesOfPermissionByPermissionId(int permissionId);

        Task<bool> IsUserHaveRole(int userId, int roleId);

        bool IsUserHaveRoleForCheckAttribute(int userId, int roleId);

        #endregion
    }
}
