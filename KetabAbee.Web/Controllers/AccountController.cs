using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KetabAbee.Application.Convertors;
using KetabAbee.Application.DTOs;
using KetabAbee.Application.Generators;
using KetabAbee.Application.Interfaces.User;
using KetabAbee.Application.Security;
using KetabAbee.Domain.Models.User;

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
                ModelState.AddModelError("UserName", "نام کاربری وارد شده تکراری است");
                return View(register);
            }

            // Check Mobile
            if (_userService.IsMobileExist(register.Mobile.Trim()))
            {
                ModelState.AddModelError("Mobile", "شماره موبایل وارد شده تکراری است");
                return View(register);
            }

            //register user
            if (_userService.RegisterUser(register))
            {
                TempData["RegisterSuccess"] = "ثبت نام شما با موفقیت انجام شد";
                TempData["InfoMessage"] = "کد تایید تلفن همراه برای شما ارسال شد.";
                return RedirectToAction("Login");
            }

            return RedirectToAction("Register");

            //TODO:send email or sms

            //TODO:return view success
        }

        #endregion

        #region Login

        [Route("Login")]
        public IActionResult Login()
        {
            return View();
        }

        #endregion

    }
}
