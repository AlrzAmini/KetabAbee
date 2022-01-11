using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KetabAbee.Application.Interfaces.Permission;

namespace KetabAbee.Web.Areas.AdminPanel.Controllers.Roles
{
    [Route("Admin/Roles")]
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

        [HttpGet]
        public IActionResult Index()
        {
            return View(_permissionService.GetRoles().ToList()); 
        }

        #endregion

        #region Delete Role

        [HttpGet("DeleteRole/{roleId}")]
        public IActionResult DeleteRole(int roleId)
        {
            if (_permissionService.DeleteRoleById(roleId))
            {
                TempData["SuccessMessage"] = "حذف نقش با موفقیت انجام شد";
                return RedirectToAction("Index");
            }

            TempData["ErrorMessage"] = "حذف نقش با شکست مواجه شد";
            return RedirectToAction("Index");
        }

        #endregion
    }
}
