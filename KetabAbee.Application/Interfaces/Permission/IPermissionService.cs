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
        #region Role

        IEnumerable<Role> GetRoles();

        void AddRolesToUser(List<int> selectedRoles, int userId);

        void EditUserRoles(List<int> selectedRoles, int userId);

        bool DeleteRoleById(int roleId);

        int AddRole(Role role);

        Role GetRoleById(int roleId);

        bool UpdateRole(Role role);

        #endregion

        #region Permission

        IEnumerable<Domain.Models.Permission.Permission> GetPermissions();

        void AddPermissionsToRole(int roleId, List<int> selectedPermissions);

        List<int> GetPermissionIdsOfRoleByRoleId(int roleId);

        void UpdatePermissionOfRole(int roleId, List<int> selectedPermission);

        bool CheckPermission(int permissionId, int userId);

        Task<bool> IsUserHaveRole(int userId, int roleId);
        bool IsUserHaveRoleForCheckAttribute(int userId, int roleId);

        #endregion
    }
}
