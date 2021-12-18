using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using KetabAbee.Application.DTOs;
using KetabAbee.Application.Interfaces.User;

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

        [HttpPost("UserPanel/EditProfile")]
        public IActionResult EditProfile(UserPanelEditInfoViewModel edit)
        {
            if (!ModelState.IsValid)
            {
                return View(edit);
            }

            bool isDone = _userService.EditUserProfile(edit);
            if (isDone)
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

        [HttpPost("UserPanel/ChangePassword")]
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

            if (!_userService.ChangePasswordInUserPanel(userName, change.Password)) return View(change);
            TempData["SuccessMessage"] = "کلمه عبور شما با موفقیت بروزرسانی شد ";
            return RedirectToAction("Dashboard");
        }

        #endregion
    }
}
