using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KetabAbee.Application.Convertors;
using KetabAbee.Application.DTOs.Admin.User;
using KetabAbee.Application.Interfaces.User;
using KetabAbee.Application.Security;
using Microsoft.AspNetCore.Http;

namespace KetabAbee.Web.Areas.AdminPanel.Controllers
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
        public IActionResult Index(FilterUsersViewModel filter)
        {
            return View(_userService.GetAllFilteredUsersInAdmin(filter));
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

        #region Add User

        [HttpGet("Admin/Users/AddUser")]
        public IActionResult AddUser()
        {
            return View();
        }

        [HttpPost("Admin/Users/AddUser"), ValidateAntiForgeryToken]
        public IActionResult AddUser(Domain.Models.User.User user, IFormFile imgFile)
        {
            if (!ModelState.IsValid)
            {
                return View(user);
            }

            // Check User Name
            if (_userService.IsUserNameExist(user.UserName))
            {
                TempData["ErrorMessage"] = "نام کاربری وارد شده تکراری است";
                return View(user);
            }

            // Check Email
            if (_userService.IsEmailExist(FixText.EmailFixer(user.Email)))
            {
                TempData["ErrorMessage"] = "شماره موبایل وارد شده تکراری است";
                return View(user);
            }

            // Check Mobile
            if (!string.IsNullOrEmpty(user.Mobile) && _userService.IsMobileExist(user.Mobile))
            {
                TempData["ErrorMessage"] = "شماره موبایل وارد شده تکراری است";
                return View(user);
            }

            // check password strength
            if (PasswordStrengthChecker.CheckStrength(user.Password) == PasswordScore.VeryWeak)
            {
                TempData["WarningMessage"] = "کلمه عبور وارد شده بسیار ضعیف است";
                TempData["InfoMessage"] = "کلمه عبور می بایست بیش از 6 کاراکتر داشته باشد";
                return View(user);
            }

            //register user
            if (!_userService.AddUser(user, imgFile)) return View(user);

            TempData["SuccessMessage"] = "ثبت کاربر با موفقیت انجام شد";
            return RedirectToAction("Index");
        }

        #endregion

        #region Edit User

        [HttpGet("Admin/Users/EditUser/{id}")]
        public IActionResult EditUser(int id) // id = user id
        {
            return View();
        }

        #endregion

        #region User Detail



        #endregion

    }
}
