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
           _permissionRepository.AddPermissionsToRole(roleId,selectedPermissions);
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

        public bool CheckPermission(int permissionId, string email)
        {
            return _permissionRepository.CheckPermission(permissionId, email);
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

        public void UpdatePermissionOfRole(int roleId, List<int> selectedPermission)
        {
            _permissionRepository.UpdatePermissionOfRole(roleId,selectedPermission);
        }

        public bool UpdateRole(Role role)
        {
            return _permissionRepository.UpdateRole(role);
        }
    }
}
