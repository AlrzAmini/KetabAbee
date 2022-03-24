using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KetabAbee.Application.Const;
using KetabAbee.Application.Convertors;
using KetabAbee.Application.DTOs.Admin.User;
using KetabAbee.Application.DTOs.Admin.Wallet;
using KetabAbee.Application.Interfaces.Permission;
using KetabAbee.Application.Interfaces.User;
using KetabAbee.Application.Interfaces.Wallet;
using KetabAbee.Application.Security;
using KetabAbee.Application.Senders;
using KetabAbee.Domain.Models.User;
using Microsoft.AspNetCore.Http;

namespace KetabAbee.Web.Areas.AdminPanel.Controllers
{
    [PermissionChecker(PerIds.AdminUsers)]
    public class AdminUserController : AdminBaseController
    {
        #region constructure

        private readonly IUserService _userService;
        private readonly IPermissionService _permissionService;
        private readonly IWalletService _walletService;
        private readonly IViewRenderService _renderService;

        public AdminUserController(IUserService userService, IPermissionService permissionService, IWalletService walletService, IViewRenderService renderService)
        {
            _userService = userService;
            _permissionService = permissionService;
            _walletService = walletService;
            _renderService = renderService;
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
        public async Task<IActionResult> DeleteUser(int id)
        {
            if (await _userService.DeleteUserById(id))
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
        public async Task<IActionResult> AddUser(AddUserFromAdminViewModel user, IFormFile imgFile, List<int> selectedRoles)
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
            var userId = await _userService.AddUser(user, imgFile);
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
        public IActionResult ChargeWallet(ChargeWalletFromAdminViewModel charge)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("ChargeWallet", new { id = charge.UserId });
            }

            // check price cant be 0
            if (charge.Amount <= 0)
            {
                TempData["ErrorMessage"] = "مبلغ وارد شده صحیح نمی باشد";
                return RedirectToAction("ChargeWallet", new { id = charge.UserId });
            }

            // charge wallet
            if (_walletService.ChargeWalletFromAdmin(charge))
            {
                TempData["SuccessMessage"] = "شارژ حساب با موفقیت انجام شد ";
                return RedirectToAction("ChargeWallet", new { id = charge.UserId });
            }

            TempData["ErrorMessage"] = "عملیات شارژ حساب انجام نشد";
            return RedirectToAction("ChargeWallet", new { id = charge.UserId });
        }

        #endregion

        #region withdraw from wallet

        [HttpPost]
        public IActionResult WithDrawFromWallet(ChargeWalletFromAdminViewModel charge) // id = user id
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("ChargeWallet", new { id = charge.UserId });
            }

            // check price cant be 0
            if (charge.Amount <= 0)
            {
                TempData["ErrorMessage"] = "مبلغ وارد شده صحیح نمی باشد";
                return RedirectToAction("ChargeWallet", new { id = charge.UserId });
            }

            // charge wallet
            if (_walletService.WithDrawWalletFromAdmin(charge))
            {
                TempData["SuccessMessage"] = "برداشت از حساب با موفقیت انجام شد ";
                return RedirectToAction("ChargeWallet", new { id = charge.UserId });
            }

            TempData["ErrorMessage"] = "عملیات برداشت انجام نشد";
            return RedirectToAction("ChargeWallet", new { id = charge.UserId });
        }

        #endregion

        #region user info

        [HttpGet("Admin/Users/Info/{userId}")]
        public async Task<IActionResult> UserInfo(int userId)
        {
            var user = await _userService.GetUserForShowInUserInfo(userId);
            if (user != null) return View(user);

            TempData["ErrorMessage"] = "کاربری یافت نشد";
            return RedirectToAction("Index");
        }

        #endregion

        #region user info collections

        #region ip's

        [HttpGet("Admin/Users/Ips")]
        public IActionResult UserIps(int userId)
        {
            var userIps = _userService.GetUserIps(userId);

            if (userIps != null && userIps.Any())
            {
                return View("_ShowUserIps", userIps);
            }
            TempData["ErrorMessage"] = "آی پی ثبت نشده است";
            return RedirectToAction("UserInfo", new { userId });
        }

        #endregion

        #region scores

        [HttpGet("Admin/Users/{userId}/Scores")]
        public IActionResult UserScores(int userId)
        {
            var userScores = _userService.GetUserBookScores(userId);
            if (userScores != null && userScores.Any()) return View(userScores);
            TempData["ErrorMessage"] = "این کاربر امتیازی ثبت نکرده است";
            return RedirectToAction("UserInfo", new { userId });
        }

        #endregion

        #region favorite books

        [HttpGet("Admin/Users/{userId}/Favorites")]
        public IActionResult UserFavoriteBooks(int userId)
        {
            var userFavBooks = _userService.GetUserFavoriteBooks(userId);
            if (userFavBooks != null && userFavBooks.Any()) return View(userFavBooks);

            TempData["ErrorMessage"] = "این کاربر محصول مورد علاقه ای ثبت نکرده است";
            return RedirectToAction("UserInfo", new { userId });
        }

        #endregion

        #region orders

        [HttpGet("Admin/Users/{userId}/Orders")]
        public IActionResult UserOrders(int userId)
        {
            var orders = _userService.GetUserOrders(userId);
            if (orders != null && orders.Any()) return View(orders);

            TempData["ErrorMessage"] = "این کاربر سفارشی ثبت نکرده است";
            return RedirectToAction("UserInfo", new { userId });
        }

        #endregion

        #region comments

        [HttpGet("Admin/Users/{userId}/Comments")]
        public IActionResult UserComments(int userId)
        {
            var comments = _userService.GetUserProductComments(userId);
            if (comments != null && comments.Any()) return View(comments);

            TempData["ErrorMessage"] = "این کاربر نظری ثبت نکرده است";
            return RedirectToAction("UserInfo", new { userId });
        }

        #endregion

        #region products

        [HttpGet("Admin/Users/{userId}/Products")]
        public IActionResult UserProducts(int userId)
        {
            var books = _userService.GetUserBooks(userId);
            if (books != null && books.Any()) return View(books);

            TempData["ErrorMessage"] = "این کاربر محصولی نخریده است";
            return RedirectToAction("UserInfo", new { userId });
        }

        #endregion

        #region tickets

        [HttpGet("Admin/Users/{userId}/Tickets")]
        public IActionResult UserTickets(int userId)
        {
            var tickets = _userService.GetUserTickets(userId);
            if (tickets != null && tickets.Any()) return View(tickets);

            TempData["ErrorMessage"] = "این کاربر تیکتی ثبت نکرده است";
            return RedirectToAction("UserInfo", new { userId });
        }

        #endregion

        #region wallets

        [HttpGet("Admin/Users/{userId}/Wallets")]
        public IActionResult UserWallets(int userId)
        {
            var wallets = _userService.GetUserWallets(userId);
            if (wallets != null && wallets.Any()) return View(wallets);

            TempData["ErrorMessage"] = "این کاربر تراکنشی ثبت نکرده است";
            return RedirectToAction("UserInfo", new { userId });
        }

        #endregion

        #region reports



        #endregion

        #endregion

        #region user info commands

        #region send message

        [HttpGet("Admin/Users/{userId}/SendM")]
        public IActionResult SendActiveMessage(int userId)
        {
            TempData["InfoMessage"] = "این بخش هنوز در دسترس نیست";
            return RedirectToAction("UserInfo", new { userId });
        }

        #endregion

        #region send email

        [HttpGet("Admin/Users/{userId}/SendE")]
        public async Task<IActionResult> SendActiveEmail(int userId)
        {
            var user = _userService.CreateNewEmailActiveCodeForUser(userId);

            if (string.IsNullOrEmpty(user.UserName))
            {
                TempData["ErrorMessage"] = "کاربر نامعتبر";
                return RedirectToAction("UserInfo", new { userId });
            }

            //send active email
            var body = _renderService.RenderToStringAsync("_ActivationEmail", user);
            await SendEmail.Send(user.Email, "کد تایید حساب کاربری در کتاب آبی", body);

            TempData["SuccessMessage"] = "ایمیل کد تایید با موفقیت ارسال شد";
            return RedirectToAction("UserInfo", new { userId });
        }

        #endregion

        #region ban & free user

        #region ban

        [HttpGet("Admin/Users/{userId}/Ban")]
        public IActionResult BanUser(int userId)
        {
            var res = _userService.BanUser(userId);
            switch (res)
            {
                case BanUserIpResult.Success:
                    TempData["SuccessMessage"] = "کاربر مسدود شد";
                    return RedirectToAction("UserInfo", new { userId });
                case BanUserIpResult.NotHaveIp:
                    TempData["InfoMessage"] = "آی پی برای مسدود کردن این کاربر ثبت نشده است";
                    return RedirectToAction("UserInfo", new { userId });
                case BanUserIpResult.Error:
                    TempData["ErrorMessage"] = "مشکلی در مسدود کردن کاربر رخ داد";
                    return RedirectToAction("UserInfo", new { userId });
                default:
                    TempData["ErrorMessage"] = "مشکلی در مسدود کردن کاربر رخ داد";
                    return RedirectToAction("UserInfo", new { userId });
            }
        }

        #endregion

        #region free

        [HttpGet("Admin/Users/{userId}/Free")]
        public IActionResult FreeUser(int userId)
        {
            var res = _userService.FreeUser(userId);
            switch (res)
            {
                case FreeUserIpResult.Success:
                    TempData["SuccessMessage"] = "کاربر آزاد شد";
                    return RedirectToAction("UserInfo", new { userId });
                case FreeUserIpResult.NotHaveIp:
                    TempData["InfoMessage"] = "آی پی برای آزاد کردن این کاربر ثبت نشده است";
                    return RedirectToAction("UserInfo", new { userId });
                case FreeUserIpResult.UserIsAlreadyFree:
                    TempData["WarningMessage"] = "کاربر در حال حاضر آزاد است";
                    return RedirectToAction("UserInfo", new { userId });
                case FreeUserIpResult.Error:
                    TempData["ErrorMessage"] = "مشکلی در آزاد کردن کاربر رخ داد";
                    return RedirectToAction("UserInfo", new { userId });
                default:
                    TempData["ErrorMessage"] = "مشکلی در آزاد کردن کاربر رخ داد";
                    return RedirectToAction("UserInfo", new { userId });
            }
        }

        #endregion

        #endregion

        #region delete user

        [HttpGet("Admin/Users/{userId}/Del")]
        public async Task<IActionResult> DeleteUserInInfo(int userId)
        {
            if (await _userService.DeleteUserById(userId))
            {
                TempData["SuccessMessage"] = "کاربر با موفقیت حذف شد";
                return RedirectToAction("Index");
            }
            TempData["ErrorMessage"] = "مشکلی در حذف کاربر رخ داد";
            return RedirectToAction("UserInfo", new { userId });
        }

        #endregion

        #region delete user comments

        [HttpGet("Admin/Users/{userId}/DelAllComments")]
        public IActionResult DeleteUserComments(int userId)
        {
            var res = _userService.DeleteUserComments(userId);
            switch (res)
            {
                case DeleteUserCommentsResult.Success:
                    TempData["SuccessMessage"] = "تمام نظرات این کاربر حذف شد";
                    return RedirectToAction("UserInfo", new { userId });
                case DeleteUserCommentsResult.NotHaveComment:
                    TempData["InfoMessage"] = "این کاربر نظری ثبت نکرده است";
                    return RedirectToAction("UserInfo", new { userId });
                case DeleteUserCommentsResult.Error:
                    TempData["ErrorMessage"] = "مشکلی در حذف نظرات رخ داد";
                    return RedirectToAction("UserInfo", new { userId });
                default:
                    TempData["ErrorMessage"] = "مشکلی در حذف نظرات رخ داد";
                    return RedirectToAction("UserInfo", new { userId });
            }
        }

        #endregion

        #region active user

        public async Task<IActionResult> ActiveUser(int userId)
        {
            return RedirectToAction("UserInfo", new { userId });
        }

        #endregion

        #region de active user

        public async Task<IActionResult> DeActiveUser(int userId)
        {
            return RedirectToAction("UserInfo", new { userId });
        }

        #endregion

        #endregion
    }
}
