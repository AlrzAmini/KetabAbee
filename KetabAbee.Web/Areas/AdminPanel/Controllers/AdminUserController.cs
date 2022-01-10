using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using KetabAbee.Application.Convertors;
using KetabAbee.Application.DTOs.Admin.User;
using KetabAbee.Application.DTOs.Admin.Wallet;
using KetabAbee.Application.Extensions;
using KetabAbee.Application.Interfaces.Permission;
using KetabAbee.Application.Interfaces.User;
using KetabAbee.Application.Interfaces.Wallet;
using KetabAbee.Application.Security;
using Microsoft.AspNetCore.Http;

namespace KetabAbee.Web.Areas.AdminPanel.Controllers
{
    public class AdminUserController : AdminBaseController
    {
        #region constructure

        private readonly IUserService _userService;
        private readonly IPermissionService _permissionService;
        private readonly IWalletService _walletService;

        public AdminUserController(IUserService userService, IPermissionService permissionService, IWalletService walletService)
        {
            _userService = userService;
            _permissionService = permissionService;
            _walletService = walletService;
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
            // All Roles
            ViewBag.Roles = _permissionService.GetRoles().ToList();

            return View();
        }

        [HttpPost("Admin/Users/AddUser"), ValidateAntiForgeryToken]
        public IActionResult AddUser(AddUserFromAdminViewModel user, IFormFile imgFile, List<int> selectedRoles)
        {
            #region Validations

            if (!ModelState.IsValid)
            {
                // Get Roles
                ViewBag.Roles = _permissionService.GetRoles().ToList();
                return View(user);
            }

            // Check User Name
            if (_userService.IsUserNameExist(user.UserName))
            {
                TempData["ErrorMessage"] = "نام کاربری وارد شده تکراری است";
                // Get Roles
                ViewBag.Roles = _permissionService.GetRoles().ToList();
                return View(user);
            }

            // Check Email
            if (_userService.IsEmailExist(FixText.EmailFixer(user.Email)))
            {
                TempData["ErrorMessage"] = "ایمیل وارد شده تکراری است";
                // Get Roles
                ViewBag.Roles = _permissionService.GetRoles().ToList();
                return View(user);
            }

            // Check Mobile
            if (!string.IsNullOrEmpty(user.Mobile) && _userService.IsMobileExist(user.Mobile))
            {
                TempData["ErrorMessage"] = "شماره موبایل وارد شده تکراری است";
                // Get Roles
                ViewBag.Roles = _permissionService.GetRoles().ToList();
                return View(user);
            }

            // check password strength
            if (PasswordStrengthChecker.CheckStrength(user.Password) == PasswordScore.VeryWeak)
            {
                TempData["WarningMessage"] = "کلمه عبور وارد شده بسیار ضعیف است";
                TempData["InfoMessage"] = "کلمه عبور می بایست بیش از 6 کاراکتر داشته باشد";
                // Get Roles
                ViewBag.Roles = _permissionService.GetRoles().ToList();
                return View(user);
            }

            #endregion

            //register user
            var userId = _userService.AddUser(user, imgFile);
            if (userId == 0) return View(user);

            // add roles to user
            _permissionService.AddRolesToUser(selectedRoles, userId);

            TempData["SuccessMessage"] = "ثبت کاربر با موفقیت انجام شد";

            return RedirectToAction("Index");
        }

        #endregion

        #region Edit User

        [HttpGet("Admin/Users/EditUser/{id}")]
        public IActionResult EditUser(int id) // id = user id
        {
            // All Roles
            ViewBag.Roles = _permissionService.GetRoles().ToList();

            return View(_userService.GetUserForEditInAdmin(id));
        }

        [HttpPost("Admin/Users/EditUser/{id}")]
        public IActionResult EditUser(EditUserViewModel user, List<int> selectedRoles) // id = user id
        {
            #region Validations

            if (!ModelState.IsValid)
            {
                // Get Roles
                ViewBag.Roles = _permissionService.GetRoles().ToList();
                return View(user);
            }

            // Check User Name
            var userName = _userService.GetUserNameByUserId(user.UserId);
            if (userName != user.UserName) // user name changed ?
            {
                if (_userService.IsUserNameExist(user.UserName))
                {
                    // Get Roles
                    ViewBag.Roles = _permissionService.GetRoles().ToList();
                    TempData["ErrorMessage"] = "نام کاربری وارد شده تکراری است";
                    user.UserRoles = selectedRoles;
                    return View(user);
                }
            }

            // Check Email
            var email = _userService.GetEmailByUserId(user.UserId);
            if (email != FixText.EmailFixer(user.Email)) // email changed ?
            {
                if (_userService.IsEmailExist(FixText.EmailFixer(user.Email)))
                {
                    // Get Roles
                    ViewBag.Roles = _permissionService.GetRoles().ToList();
                    TempData["ErrorMessage"] = "ایمیل وارد شده تکراری است";
                    user.UserRoles = selectedRoles;
                    return View(user);
                }
            }

            // Check Mobile
            var mobile = _userService.GetMobileByUserId(user.UserId);
            if (mobile != user.Mobile) // mobile changed ?
            {
                if (!string.IsNullOrEmpty(user.Mobile) && _userService.IsMobileExist(user.Mobile))
                {
                    // Get Roles
                    ViewBag.Roles = _permissionService.GetRoles().ToList();
                    TempData["ErrorMessage"] = "شماره موبایل وارد شده تکراری است";
                    user.UserRoles = selectedRoles;
                    return View(user);
                }
            }

            // check password strength
            if (!string.IsNullOrEmpty(user.Password))
            {
                if (PasswordStrengthChecker.CheckStrength(user.Password) == PasswordScore.VeryWeak || PasswordStrengthChecker.CheckStrength(user.Password) == PasswordScore.Blank)
                {
                    // Get Roles
                    ViewBag.Roles = _permissionService.GetRoles().ToList();
                    TempData["WarningMessage"] = "کلمه عبور وارد شده بسیار ضعیف است";
                    TempData["InfoMessage"] = "کلمه عبور می بایست بیش از 6 کاراکتر داشته باشد";
                    user.UserRoles = selectedRoles;
                    return View(user);
                }
            }


            #endregion

            if (_userService.EditUserFromAdmin(user))
            {
                //Edit Roles
                _permissionService.EditUserRoles(selectedRoles, user.UserId);

                TempData["SuccessMessage"] = "ویرایش کاربر با موفقیت انجام شد";
                return RedirectToAction("Index");
            }
            TempData["ErrorMessage"] = "ویرایش کاربر با شکست مواجه شد";
            return RedirectToAction("Index");
        }

        #endregion

        #region Charge Wallet

        [HttpGet("Admin/Users/ChargeWallet/{id}")]
        public IActionResult ChargeWallet(int id) // id = user id
        {
            return View(_userService.GetChargeInfoForAdmin(id));
        }

        [HttpPost("Admin/Users/ChargeWallet/{userId}")]
        public IActionResult ChargeWallet(ChargeWalletFromAdminViewModel charge) // id = user id
        {
            if (!ModelState.IsValid)
            {
                var newCharge = _userService.GetChargeInfoForAdmin(charge.UserId);
                return View(newCharge);
            }

            // check price cant be 0
            if (charge.Amount <= 0)
            {
                TempData["ErrorMessage"] = "مبلغ وارد شده صحیح نمی باشد";
                var newCharge = _userService.GetChargeInfoForAdmin(charge.UserId);
                return View(newCharge);
            }

            // charge wallet
            if (_walletService.ChargeWalletFromAdmin(charge))
            {
                TempData["SuccessMessage"] = "شارژ حساب با موفقیت انجام شد ";
                var newCharge = _userService.GetChargeInfoForAdmin(charge.UserId);
                return View(newCharge);
            }

            TempData["ErrorMessage"] = "عملیات شارژ حساب انجام نشد";
            var oCharge = _userService.GetChargeInfoForAdmin(charge.UserId);
            return View(oCharge);
        }

        #endregion

        #region withdraw from wallet

        [HttpPost]
        public IActionResult WithDrawFromWallet(ChargeWalletFromAdminViewModel charge) // id = user id
        {
            if (!ModelState.IsValid)
            {
                var newCharge = _userService.GetChargeInfoForAdmin(charge.UserId);
                return View("ChargeWallet", newCharge);
            }

            // check price cant be 0
            if (charge.Amount <= 0)
            {
                TempData["ErrorMessage"] = "مبلغ وارد شده صحیح نمی باشد";
                var newCharge = _userService.GetChargeInfoForAdmin(charge.UserId);
                return View("ChargeWallet", newCharge);
            }

            // charge wallet
            if (_walletService.WithDrawWalletFromAdmin(charge))
            {
                TempData["SuccessMessage"] = "برداشت از حساب با موفقیت انجام شد ";
                var newCharge = _userService.GetChargeInfoForAdmin(charge.UserId);
                return View("ChargeWallet", newCharge);
            }

            TempData["ErrorMessage"] = "عملیات برداشت انجام نشد";
            var oCharge = _userService.GetChargeInfoForAdmin(charge.UserId);
            return RedirectToAction("ChargeWallet", oCharge);
        }

        #endregion

    }
}
