using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using GoogleReCaptcha.V3.Interface;
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
        private readonly ICaptchaValidator _captchaValidator;

        public AccountController(IUserService userService, ICaptchaValidator captchaValidator)
        {
            _userService = userService;
            _captchaValidator = captchaValidator;
        }

        #region Register

        [HttpGet("Register")]
        public IActionResult Register()
        {
            if (User.Identity.IsAuthenticated) return Redirect("/");

            return View();
        }

        [HttpPost("Register"), ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel register)
        {
            if (!await _captchaValidator.IsCaptchaPassedAsync(register.Captcha))
            {
                TempData["ErrorMessage"] = "لطفا احراز هویت کپچا را انجام دهید";
                return View(register);
            }

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
            if (_userService.IsEmailExist(FixText.EmailFixer(register.Email)))
            {
                TempData["ErrorMessage"] = "شماره موبایل وارد شده تکراری است";
                return View(register);
            }

            //register user
            if (_userService.RegisterUser(register))
            {
                TempData["SuccessMessage"] = "ثبت نام شما با موفقیت انجام شد";
                TempData["InfoMessage"] = "ایمیل فعالسازی برای شما ارسال شد";
                TempData["WarningMessage"] = "ممکن است عملیات ارسال ایمیل فعال سازی دقایقی طول بکشد";
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

        [HttpPost("Login"), ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel login)
        {
            if (!await _captchaValidator.IsCaptchaPassedAsync(login.Captcha))
            {
                TempData["ErrorMessage"] = "احراز هویت کپچا انجام نشد . دوباره تلاش کنید";
                return View(login);
            }

            if (!ModelState.IsValid)
            {
                return View(login);
            }

            var user = _userService.GetUserForLogin(login);

            if (user != null)
            {
                if (!user.IsEmailActive)
                {
                    TempData["WarningMessage"] = "حساب کاربری شما فعال نشده است";
                    return View(login);
                }

                var claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.NameIdentifier,user.UserId.ToString()),
                    new Claim(ClaimTypes.Name,user.UserName),
                    new Claim(ClaimTypes.Email,user.Email)
                };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                var properties = new AuthenticationProperties()
                {
                    IsPersistent = login.RememberMe
                };

                // command for login user
                await HttpContext.SignInAsync(principal, properties);

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
