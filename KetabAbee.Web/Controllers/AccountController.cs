using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using GoogleReCaptcha.V3.Interface;
using KetabAbee.Application.Convertors;
using KetabAbee.Application.DTOs;
using KetabAbee.Application.DTOs.ActiveAccount;
using KetabAbee.Application.Extensions;
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
        #region Constructor

        private readonly IUserService _userService;
        private readonly ICaptchaValidator _captchaValidator;
        private readonly IViewRenderService _renderService;

        public AccountController(IUserService userService, ICaptchaValidator captchaValidator, IViewRenderService renderService)
        {
            _userService = userService;
            _captchaValidator = captchaValidator;
            _renderService = renderService;
        }

        #endregion

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
                TempData["ErrorSwal"] = "احراز هویت کپچا انجام نشد چند لحظه دیگر تلاش کنید";
                return View(register);
            }

            if (!ModelState.IsValid)
            {
                return View(register);
            }

            // Check User Name
            if (_userService.IsUserNameExist(register.UserName))
            {
                TempData["ErrorSwal"] = "خودمم دوس داشتم قبول کنم ولی این اسم رو قبلا یکی گذاشته برا خودش";
                return View(register);
            }

            // Check Email
            if (_userService.IsEmailExist(FixText.EmailFixer(register.Email)))
            {
                TempData["ErrorSwal"] = "شرمنده بخدا ولی یکی این ایمیل رو زودتر وارد کرده";
                return View(register);
            }

            // check password strength
            if (PasswordStrengthChecker.CheckStrength(register.Password) == PasswordScore.VeryWeak)
            {
                TempData["ErrorSwal"] = "همه جا همینجوری رمز وارد میکنی ستون ؟";
                TempData["InfoMessage"] = "کلمه عبور می بایست حداقل 6 کاراکتر داشته باشد";
                return View(register);
            }

            //register user
            register.UserIp = HttpContext.GetUserIp();
            var user = await _userService.RegisterUser(register);
            if (user == null) return RedirectToAction("Register");
            TempData["SuccessSwal"] = "ثبت نام شما با موفقیت انجام شد";
            TempData["InfoMessage"] = "لطفا با استفاده از کد ارسال شده حساب خود را فعالسازی کنید";
            TempData["WarningMessage"] = "ممکن است عملیات ارسال ایمیل فعال سازی دقایقی طول بکشد";

            //send active email
            var body = _renderService.RenderToStringAsync("_ActivationEmail", user);
            await SendEmail.Send(user.Email, "کد تایید حساب کاربری در کتاب آبی", body);

            return RedirectToAction("Activator");
        }

        #endregion

        #region Login

        [HttpGet("Login")]
        public IActionResult Login(string returnUrl)
        {
            if (User.Identity.IsAuthenticated) return Redirect("/");

            ViewData["returnUrl"] = returnUrl;
            return View();
        }

        [HttpPost("Login"), ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel login, string returnUrl = "/")
        {
            if (!await _captchaValidator.IsCaptchaPassedAsync(login.Captcha))
            {
                TempData["ErrorSwal"] = "احراز هویت کپچا انجام نشد . دوباره تلاش کنید";
                ViewData["returnUrl"] = returnUrl;
                return View(login);
            }

            if (!ModelState.IsValid)
            {
                ViewData["returnUrl"] = returnUrl;
                return View(login);
            }

            if (!login.AcceptRules)
            {
                TempData["WarningSwal"] = "برای ورود به سایت میبایست با قوانین سایت موافقت کنید";
                ViewData["returnUrl"] = returnUrl;
                return View(login);
            }

            var user = await _userService.GetUserForLogin(login);

            if (user != null)
            {
                if (!user.IsEmailActive)
                {
                    TempData["WarningSwal"] = "حساب کاربری شما فعال نشده است";
                    ViewData["returnUrl"] = returnUrl;
                    return View(login);
                }
                if (user.IsBanned)
                {
                    TempData["WarningSwal"] = "حساب کاربری شما مسدود شده است";
                    ViewData["returnUrl"] = returnUrl;
                    return View(login);
                }

                var claims = new List<Claim>
                {
                    new(ClaimTypes.NameIdentifier,user.UserId.ToString()),
                    new(ClaimTypes.Name,user.UserName),
                    new(ClaimTypes.Email,user.Email)
                };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                var properties = new AuthenticationProperties
                {
                    IsPersistent = login.RememberMe
                };

                // command for login user
                await HttpContext.SignInAsync(principal, properties);

                await _userService.AddUserIp(new UserIp
                {
                    Ip = HttpContext.GetUserIp(),
                    UserId = user.UserId
                });
                user.IsOnline = true;
                _userService.UpdateUser(user);

                if (returnUrl != "/")
                {
                    return Redirect(returnUrl);
                }

                return Redirect("/");
            }

            ViewData["returnUrl"] = returnUrl;
            TempData["ErrorSwal"] = "نام کاربری یا کلمه عبور اشتباه است";
            return View(login);

        }

        #endregion

        #region Logout

        [HttpGet("Logout")]
        public IActionResult Logout(string url)
        {
            var user = _userService.GetUserByEmail(User.GetUserEmail());
            user.IsOnline = false;
            _userService.UpdateUser(user);

            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect(url ?? "/");
        }

        #endregion

        #region Active Account by Email

        //[HttpGet("ActiveByEmail/{id}")]
        //public IActionResult ActiveByEmail(string id) // id = active code
        //{
        //    ViewBag.IsActive = _userService.ActiveAccountByEmail(id);
        //    return View();
        //}

        #endregion

        #region Active Email By 5th Code

        [HttpGet("Activator")]
        public IActionResult Activator()
        {
            return View();
        }

        [HttpPost("Activator"), ValidateAntiForgeryToken]
        public IActionResult Activator(ActiveEmailBy5ThCodeViewModel actCode)
        {
            if (!ModelState.IsValid)
            {
                return View(actCode);
            }

            if (_userService.EmailActivatorBy5ThCode(actCode.ActiveCode))
            {
                TempData["SuccessSwal"] = "حساب شما با موفقیت فعالسازی شد";
                return RedirectToAction("Login");
            }

            TempData["ErrorSwal"] = "مشکلی در فعالسازی حساب رخ داده است کد فعالسازی را بررسی کنید";
            TempData["WarningMessage"] = "ممکن است حساب کاربری شما از قبل فعالسازی شده باشد بررسی کنید";
            return View(actCode);
        }

        #endregion

        #region Forgot Password

        [HttpGet("ForgotPassword")]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost("ForgotPassword"), ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel forgot)
        {
            if (!await _captchaValidator.IsCaptchaPassedAsync(forgot.Captcha))
            {
                TempData["ErrorSwal"] = "احراز هویت کپچا انجام نشد . دوباره تلاش کنید";
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
                TempData["ErrorSwal"] = "کاربری با این ایمیل ثبت نشده است";
                return View(forgot);
            }

            string body = _renderService.RenderToStringAsync("_ForgotPasswordEmail", user);
            await SendEmail.Send(user.Email, "بازیابی رمز عبور", body);
            TempData["SuccessSwal"] = "ایمیل بازیابی رمز عبور برای شما ارسال گردید";

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

        [HttpPost("ResetPasswordByEmail/{id}"), ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPasswordByEmail(ResetPasswordViewModel reset)
        {
            if (!await _captchaValidator.IsCaptchaPassedAsync(reset.Captcha))
            {
                TempData["ErrorSwal"] = "احراز هویت کپچا انجام نشد . دوباره تلاش کنید";
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

            // check password strength
            if (PasswordStrengthChecker.CheckStrength(reset.Password) == PasswordScore.VeryWeak)
            {
                TempData["WarningSwal"] = "کلمه عبور وارد شده بسیار ضعیف است";
                TempData["InfoMessage"] = "کلمه عبور می بایست حداقل 6 کاراکتر داشته باشد";
                return View(reset);
            }

            user.Password = PasswordHasher.EncodePasswordMd5(reset.Password.Sanitizer());
            user.EmailActivationCode = new Random().Next(10000, 99999).ToString();

            _userService.UpdateUser(user);

            TempData["SuccessSwal"] = "رمز عبور شما با موفقیت تغییر کرد";

            return RedirectToAction("Login");

        }

        #endregion
    }
}
