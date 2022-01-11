using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KetabAbee.Application.Interfaces.Permission;

namespace KetabAbee.Web.Areas.AdminPanel.Controllers.Roles
{
    public class AdminRoleController : AdminBaseController
    {
        #region constructor

        private readonly IPermissionService _permissionService;

        public AdminRoleController(IPermissionService permissionService)
        {
            _permissionService = permissionService;
        }

        #endregion

        #region List Roles

        [HttpGet("Admin/Roles")]
        public IActionResult Index()
        {
            return View(_permissionService.GetRoles().ToList()); 
        }

        #endregion
    }
}
