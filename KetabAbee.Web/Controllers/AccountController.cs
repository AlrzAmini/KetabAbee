using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using KetabAbee.Application.Convertors;
using KetabAbee.Application.DTOs;
using KetabAbee.Application.Generators;
using KetabAbee.Application.Interfaces.User;
using KetabAbee.Application.Security;
using KetabAbee.Domain.Models.User;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace KetabAbee.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserService _userService;

        public AccountController(IUserService userService)
        {
            _userService = userService;
        }

        #region Register

        [HttpGet("Register")]
        public IActionResult Register()
        {
            if (User.Identity.IsAuthenticated) return Redirect("/");

            return View();
        }

        [HttpPost("Register") , ValidateAntiForgeryToken]
        public IActionResult Register(RegisterViewModel register)
        {
            if (!ModelState.IsValid)
            {
                return View(register);
            }

            // Check User Name
            if (_userService.IsUserNameExist(register.UserName))
            {
                TempData["ErrorMessage"] = "نام کاربری وارد شده تکراری است";
                return View(register);
            }

            // Check Mobile
            if (_userService.IsMobileExist(register.Mobile.Trim()))
            {
                TempData["ErrorMessage"] = "شماره موبایل وارد شده تکراری است";
                return View(register);
            }

            //register user
            if (_userService.RegisterUser(register))
            {
                TempData["SuccessMessage"] = "ثبت نام شما با موفقیت انجام شد";
                TempData["InfoMessage"] = "کد تایید تلفن همراه برای شما ارسال شد";
                return RedirectToAction("Login");
            }

            return RedirectToAction("Register");

            //TODO:send email or sms

            //TODO:return view success
        }

        #endregion

        #region Login

        [HttpGet("Login")]
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated) return Redirect("/");
            return View();
        }

        [HttpPost("Login"),ValidateAntiForgeryToken]
        public IActionResult Login(LoginViewModel login)
        {
            if (!ModelState.IsValid)
            {
                return View(login);
            }

            var user = _userService.GetUserForLogin(login);

            if (user != null)
            {
                if (!user.IsMobileActive)
                {
                    TempData["WarningMessage"] = "حساب کاربری شما فعال نشده است";
                    return View(login);
                }

                var claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.NameIdentifier,user.UserId.ToString()),
                    new Claim(ClaimTypes.Name,user.UserName),
                    new Claim(ClaimTypes.MobilePhone,user.Mobile),
                };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                var properties = new AuthenticationProperties()
                {
                    IsPersistent = login.RememberMe
                };

                // command for login user
                HttpContext.SignInAsync(principal, properties);

                TempData["SuccessMessage"] = "خوش آمدید";
                return Redirect("/");
            }

            TempData["ErrorMessage"] = "نام کاربری یا کلمه عبور اشتباه است";
            return View(login);

        }

        #endregion

        #region Logout

        [HttpGet("Logout")]
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect("/");
        }

        #endregion

    }
}
