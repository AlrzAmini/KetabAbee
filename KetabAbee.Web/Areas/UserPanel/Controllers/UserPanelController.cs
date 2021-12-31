using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using KetabAbee.Application.Convertors;
using KetabAbee.Application.DTOs;
using KetabAbee.Application.Interfaces.User;
using KetabAbee.Application.Security;

namespace KetabAbee.Web.Areas.UserPanel.Controllers
{
    public class UserPanelController : UserPanelBaseController
    {
        #region constructor

        private readonly IUserService _userService;

        public UserPanelController(IUserService userService)
        {
            _userService = userService;
        }

        #endregion

        #region Dashborad

        [HttpGet("UserPanel/Dashboard")]
        public IActionResult Dashboard()
        {
            var userInfo = _userService.GetInfoByUserEmail(User.FindFirstValue(ClaimTypes.Email));

            return View(userInfo);
        }

        #endregion

        #region Edit Profile

        [HttpGet("UserPanel/EditProfile")]
        public IActionResult EditProfile()
        {
            var infoForEdit = _userService.GetInfoForEditInUserPanel(User.Identity.Name);

            return View(infoForEdit);
        }

        [HttpPost("UserPanel/EditProfile") , ValidateAntiForgeryToken]
        public IActionResult EditProfile(UserPanelEditInfoViewModel edit)
        {
            if (!ModelState.IsValid)
            {
                return View(edit);
            }

            edit.BirthDay = edit.BirthDay?.ToString(CultureInfo.InvariantCulture).StringShamsiToMiladi();

            if (_userService.EditUserProfile(edit))
            {
                TempData["SuccessMessage"] = "پروفایل شما با موفقیت بروزرسانی شد";

                return RedirectToAction("Dashboard");
            }

            TempData["ErrorMessage"] = "خطایی در هنگام ویرایش اطلاعات شما رخ داد";

            return RedirectToAction("Dashboard");

        }

        #endregion

        #region Change Password

        [HttpGet("UserPanel/ChangePassword")]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost("UserPanel/ChangePassword") , ValidateAntiForgeryToken]
        public IActionResult ChangePassword(ChangePasswordViewModel change)
        {
            if (!ModelState.IsValid)
            {
                return View(change);
            }

            string userName = User.Identity.Name;

            if (!_userService.IsOldPasswordCorrect(userName,change.OldPassword))
            {
                TempData["ErrorMessage"] = "کلمه عبور فعلی شما همخوانی ندارد ";
                return View(change);
            }

            // check password strength
            if (PasswordStrengthChecker.CheckStrength(change.Password) == PasswordScore.VeryWeak)
            {
                TempData["WarningMessage"] = "کلمه عبور جدید بسیار ضعیف است";
                TempData["InfoMessage"] = "کلمه عبور می بایست بیش از 6 کاراکتر داشته باشد";
                return View(change);
            }

            if (!_userService.ChangePasswordInUserPanel(userName, change.Password)) return View(change);
            TempData["SuccessMessage"] = "کلمه عبور شما با موفقیت بروزرسانی شد ";
            return RedirectToAction("Dashboard");
        }

        #endregion
    }
}
