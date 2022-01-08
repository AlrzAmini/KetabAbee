using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KetabAbee.Data.Context;
using KetabAbee.Domain.Interfaces;
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
    }
}
