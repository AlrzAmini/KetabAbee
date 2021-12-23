using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KetabAbee.Application.Interfaces.User;

namespace KetabAbee.Web.Areas.AdminPanel.Controllers.User
{
    public class AdminUserController : AdminBaseController
    {
        #region constructure

        private readonly IUserService _userService;

        public AdminUserController(IUserService userService)
        {
            _userService = userService;
        }

        #endregion

        #region Users Index

        [HttpGet("Admin/Users")]
        public IActionResult Index()
        {
            return View(_userService.GetAllUsersForAdmin());
        }

        #endregion

        #region Delete User

        [HttpGet("Admin/Users/DeleteUser/{id}")]
        public IActionResult DeleteUser(int id)
        {
            if (_userService.DeleteUserById(id))
            {
                TempData["SuccessMessage"] = "حذف کاربر با موفقیت انجام شد";
                return RedirectToAction("Index");
            }
            TempData["ErrorMessage"] = "عملیات حذف کاربر با شکست مواجه شد";
            return RedirectToAction("Index");
        }

        #endregion

    }
}
