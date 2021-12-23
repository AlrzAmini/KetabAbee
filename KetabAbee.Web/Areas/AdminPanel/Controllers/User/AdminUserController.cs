using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KetabAbee.Application.Interfaces.User;

namespace KetabAbee.Web.Areas.AdminPanel.Controllers.User
{
    public class AdminUserController : AdminBaseController
    {
        #region constructure

        private readonly IUserService _userService;

        public AdminUserController(IUserService userService)
        {
            _userService = userService;
        }

        #endregion

        [HttpGet("Admin/Users")]
        public IActionResult Index()
        {
            return View(_userService.GetAllUsers());
        }
    }
}
