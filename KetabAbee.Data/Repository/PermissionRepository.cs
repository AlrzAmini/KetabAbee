using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KetabAbee.Data.Context;
using KetabAbee.Domain.Interfaces;
using KetabAbee.Domain.Models.Permission;
using KetabAbee.Domain.Models.User;

namespace KetabAbee.Data.Repository
{
    public class PermissionRepository : IPermissionRepository
    {
        private readonly KetabAbeeDBContext _context;

        public PermissionRepository(KetabAbeeDBContext context)
        {
            _context = context;
        }

        public IEnumerable<Role> GetRoles()
        {
            return _context.Roles;
        }

        public void AddRolesToUser(List<int> selectedRoleIds, int userId)
        {
            // add selected roles to user
            foreach (var roleId in selectedRoleIds)
            {
                _context.UserRoles.Add(new UserRole()
                {
                    UserId = userId,
                    RoleId = roleId
                });
            }

            _context.SaveChanges();
        }

        public void EditUserRoles(List<int> selectedRoleIds, int userId)
        {
            // remove old user roles
            _context.UserRoles.Where(r => r.UserId == userId)
                .ToList()
                .ForEach(r => _context.UserRoles.Remove(r));

            // add new roles to user
            AddRolesToUser(selectedRoleIds, userId);
        }
        public Role GetRoleById(int roleId)
        {
            return _context.Roles.Find(roleId);
        }

        public bool DeleteRole(int roleId)
        {
            try
            {
                var role = GetRoleById(roleId);
                role.IsDelete = true;
                UpdateRole(role);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool UpdateRole(Role role)
        {
            try
            {
                _context.Roles.Update(role);
                _context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public int AddRole(Role role)
        {
            role.IsDelete = false;
            _context.Roles.Add(role);
            _context.SaveChanges();
            return role.RoleId;
        }

        public IEnumerable<Permission> GetPermissions()
        {
            return _context.Permissions;
        }

        public void AddPermissionsToRole(int roleId, List<int> selectedPermissions)
        {
            foreach (var perId in selectedPermissions)
            {
                _context.RolePermissions.Add(new RolePermission
                {
                    RoleId = roleId,
                    PermissionId = perId
                });
            }

            _context.SaveChanges();
        }

        public List<int> GetPermissionIdsOfRoleByRoleId(int roleId)
        {
            return _context.RolePermissions.Where(rp => rp.RoleId == roleId)
                .Select(r => r.PermissionId).ToList();
        }

        public void UpdatePermissionOfRole(int roleId, List<int> selectedPermission)
        {
            // remove old permissions of role
            _context.RolePermissions
               .Where(p => p.RoleId == roleId)
               .ToList()
               .ForEach(p => _context.RolePermissions.Remove(p));

            // add new roles
            AddPermissionsToRole(roleId, selectedPermission);
        }
    }
}
