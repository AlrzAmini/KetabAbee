﻿using System;
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

        public void AddRolesToUser(List<int> selectedRoles, int userId)
        {
            _permissionRepository.AddRolesToUser(selectedRoles,userId);
        }

        public IEnumerable<Role> GetRoles()
        {
            return _permissionRepository.GetRoles();
        }
    }
}
