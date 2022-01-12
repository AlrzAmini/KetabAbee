using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KetabAbee.Application.Interfaces.Permission;
using KetabAbee.Domain.Models.User;

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

        #region Add Role

        [HttpGet("AddRole")]
        public IActionResult AddRole()
        {
            ViewBag.Permissions = _permissionService.GetPermissions().ToList();
            return View();
        }

        [HttpPost("AddRole")]
        public IActionResult AddRole(Role role, List<int> selectedPermission)
        {
            if (!ModelState.IsValid)
            {
                return View(role);
            }

            var roleId = _permissionService.AddRole(role);
            if (roleId != 0)
            {
                _permissionService.AddPermissionsToRole(roleId,selectedPermission);

                TempData["SuccessMessage"] = "افزودن نقش با موفقیت انجام شد";
                return RedirectToAction("Index");
            }

            TempData["ErrorMessage"] = "افزودن نقش با شکست مواجه شد";
            return RedirectToAction("Index");
        }

        #endregion

        #region Edit Role

        [HttpGet("EditRole/{roleId}")]
        public IActionResult EditRole(int roleId)
        {
            return View(_permissionService.GetRoleById(roleId));
        }

        [HttpPost("EditRole/{roleId}")]
        public IActionResult EditRole(Role role)
        {
            if (!ModelState.IsValid)
            {
                return View(role);
            }

            if (_permissionService.UpdateRole(role))
            {
                TempData["SuccessMessage"] = "ویرایش نقش با موفقیت انجام شد";
                return RedirectToAction("Index");
            }
            TempData["ErrorMessage"] = "ویرایش نقش با شکست مواجه شد";
            return RedirectToAction("Index");
        }

        #endregion
    }
}
