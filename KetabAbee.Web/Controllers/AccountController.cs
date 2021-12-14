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

        [Route("Register")]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [Route("Register")]
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

            // Check Email
            if (_userService.IsEmailExist(FixText.EmailFixer(register.Email)))
            {
                ModelState.AddModelError("Email", "ایمیل وارد شده تکراری است");
                return View(register);
            }

            //register user
            var user = new User()
            {
                UserName = register.UserName,
                Email = FixText.EmailFixer(register.Email),
                Password = PasswordHasher.EncodePasswordMd5(register.Password),
                RegisterDate = DateTime.Now,
                IsActive = false,
                IsDelete = false,
                ActivationCode = CodeGenerator.GenerateUniqCode()
            };
            _userService.AddUser(user);

            //TODO:send email

            //TODO:return view success

            return Redirect("/");
        }

        #endregion

    }
}
