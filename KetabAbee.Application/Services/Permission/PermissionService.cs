using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KetabAbee.Application.Interfaces.Permission;
using KetabAbee.Domain.Interfaces;
using KetabAbee.Domain.Models.User;

namespace KetabAbee.Application.Services.Permission
{
    public class PermissionService : IPermissionService
    {
        private readonly IPermissionRepository _permissionRepository;

        public PermissionService(IPermissionRepository permissionRepository)
        {
            _permissionRepository = permissionRepository;
        }

        public void AddPermissionsToRole(int roleId, List<int> selectedPermissions)
        {
            if (selectedPermissions != null && selectedPermissions.Any())
            {
                _permissionRepository.AddPermissionsToRole(roleId, selectedPermissions);
            }
        }

        public int AddRole(Role role)
        {
            try
            {
                return _permissionRepository.AddRole(role);
            }
            catch
            {
                return 0;
            }
        }

        public void AddRolesToUser(List<int> selectedRoles, int userId)
        {
            _permissionRepository.AddRolesToUser(selectedRoles, userId);
        }

        public bool CheckPermission(int permissionId, int userId)
        {
            var userRoles = _permissionRepository.GetUserRolesByUserId(userId);
            if (userRoles == null) return false;
            if (!userRoles.Any()) return false;
            var rolesOfPermission = _permissionRepository.GetRolesOfPermissionByPermissionId(permissionId);

            return rolesOfPermission.Any(r => userRoles.Contains(r));
        }

        public bool DeleteRoleById(int roleId)
        {
            return _permissionRepository.DeleteRole(roleId);
        }

        public void EditUserRoles(List<int> selectedRoles, int userId)
        {
            _permissionRepository.EditUserRoles(selectedRoles, userId);
        }

        public List<int> GetPermissionIdsOfRoleByRoleId(int roleId)
        {
            return _permissionRepository.GetPermissionIdsOfRoleByRoleId(roleId);
        }

        public IEnumerable<Domain.Models.Permission.Permission> GetPermissions()
        {
            return _permissionRepository.GetPermissions();
        }

        public Role GetRoleById(int roleId)
        {
            return _permissionRepository.GetRoleById(roleId);
        }

        public IEnumerable<Role> GetRoles()
        {
            return _permissionRepository.GetRoles();
        }

        public async Task<bool> IsUserHaveRole(int userId, int roleId)
        {
            return await _permissionRepository.IsUserHaveRole(userId, roleId);
        }

        public bool IsUserHaveRoleForCheckAttribute(int userId, int roleId)
        {
            return _permissionRepository.IsUserHaveRoleForCheckAttribute(userId, roleId);
        }

        public void UpdatePermissionOfRole(int roleId, List<int> selectedPermission)
        {
            // remove role permissions
            _permissionRepository.RemovePermissionsOfRole(roleId);

            if (selectedPermission != null && selectedPermission.Any())
            {
                // add new roles
                _permissionRepository.AddPermissionsToRole(roleId, selectedPermission);
            }
        }

        public bool UpdateRole(Role role)
        {
            return _permissionRepository.UpdateRole(role);
        }
    }
}
