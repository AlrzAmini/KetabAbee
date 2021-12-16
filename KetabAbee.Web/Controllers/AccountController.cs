﻿using Microsoft.AspNetCore.Mvc;
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
using KetabAbee.Application.Senders;
using KetabAbee.Domain.Models.User;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace KetabAbee.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserService _userService;
        private readonly ICaptchaValidator _captchaValidator;
        private readonly IViewRenderService _renderService;

        public AccountController(IUserService userService, ICaptchaValidator captchaValidator, IViewRenderService renderService)
        {
            _userService = userService;
            _captchaValidator = captchaValidator;
            _renderService = renderService;
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
            var user = _userService.RegisterUser(register);
            if (user == null) return RedirectToAction("Register");
            TempData["SuccessMessage"] = "ثبت نام شما با موفقیت انجام شد";
            TempData["InfoMessage"] = "ایمیل فعالسازی برای شما ارسال شد";
            TempData["WarningMessage"] = "ممکن است عملیات ارسال ایمیل فعال سازی دقایقی طول بکشد";

            //send active email
            string body = _renderService.RenderToStringAsync("_ActivationEmail", user);
            SendEmail.Send(user.Email,"فعالسازی حساب کاربری در کتاب آبی",body);

            return RedirectToAction("Login");
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

        #region Active Account Email

        [HttpGet("ActiveByEmail/{id}")]
        public IActionResult ActiveByEmail(string id)
        {
            ViewBag.IsActive = _userService.ActiveAccountByEmail(id);
            return View();
        }

        #endregion

        #region Forgot Password

        [HttpGet("ForgotPassword")]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel forgot)
        {
            if (!await _captchaValidator.IsCaptchaPassedAsync(forgot.Captcha))
            {
                TempData["ErrorMessage"] = "احراز هویت کپچا انجام نشد . دوباره تلاش کنید";
                return View(forgot);
            }

            if (!ModelState.IsValid)
            {
                return View(forgot);
            }

            var user = _userService.GetUserByEmail(FixText.EmailFixer(forgot.Email));

            // check user exist
            if (user == null)
            {
                TempData["ErrorMessage"] = "کاربری با این ایمیل ثبت نشده است";
                return View(forgot);
            }

            string body = _renderService.RenderToStringAsync("_ForgotPasswordEmail", user);
            SendEmail.Send(user.Email, "بازیابی رمز عبور", body);
            TempData["SuccessMessage"] = "ایمیل بازیابی رمز عبور برای شما ارسال گردید";

            return View();
        }

        #endregion

        #region Reset Password

        [HttpGet("ResetPasswordByEmail/{id}")]
        public IActionResult ResetPasswordByEmail(string id) // id = email Activation code
        {
            return View(new ResetPasswordViewModel()
            {
                EmailActiveCode = id
            });
        }

        [HttpPost("ResetPasswordByEmail/{id}")]
        public async Task<IActionResult> ResetPasswordByEmail(ResetPasswordViewModel reset)
        {
            if (!await _captchaValidator.IsCaptchaPassedAsync(reset.Captcha))
            {
                TempData["ErrorMessage"] = "احراز هویت کپچا انجام نشد . دوباره تلاش کنید";
                return View(reset);
            }

            if (!ModelState.IsValid)
            {
                return View(reset);
            }

            var user = _userService.GetUserByEmailActivationCode(reset.EmailActiveCode);

            if (user == null)
            {
                return NotFound();
            }

            user.Password = PasswordHasher.EncodePasswordMd5(reset.Password);

           await _userService.UpdateUser(user);

           TempData["SuccessMessage"] = "رمز عبور شما با موفقیت تغییر کرد";

            return RedirectToAction("Login");

        }

        #endregion


    }
}
