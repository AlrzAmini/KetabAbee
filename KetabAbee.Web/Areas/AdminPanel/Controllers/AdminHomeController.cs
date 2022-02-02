using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KetabAbee.Application.Extensions;
using KetabAbee.Application.Interfaces.Product;
using KetabAbee.Application.Interfaces.User;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace KetabAbee.Web.Areas.AdminPanel.Controllers
{
    public class AdminHomeController : AdminBaseController
    {
        private readonly IProductService _productService;
        private readonly IUserService _userService;

        public AdminHomeController(IProductService productService, IUserService userService)
        {
            _productService = productService;
            _userService = userService;
        }

        #region Index

        [HttpGet("Admin")]
        public IActionResult Home()
        {
            ViewBag.Profile = _userService.GetInfoByUserEmail(User.GetUserEmail());
            var lastMonthUsers = _userService.GetLastNDaysUsers(30).Count();
            ViewBag.LastMonthUsers = lastMonthUsers;
            var lastWeekUsers = _userService.GetLastNDaysUsers(7).Count();
            ViewBag.LastWeekUsers = lastWeekUsers;
            var lastDayUsers = _userService.GetLastNDaysUsers(1).Count();
            ViewBag.LastDayUsers = lastDayUsers;

            return View();
        }

        #endregion

        #region Json

        public IActionResult GetSubGroups(int id) // id = groupId
        {
            var list = new List<SelectListItem>
            {
                new(){Text = "دسته بندی فرعی اول را انتخاب کنید",Value = ""}
            };
            list.AddRange(_productService.GetSubGroupsForAddBook(id));
            return Json(new SelectList(list, "Value", "Text"));
        }

        public IActionResult GetSubGroups2(int id) // id = sub id 1
        {
            var list = new List<SelectListItem>
            {
                new(){Text = "دسته بندی فرعی دوم را انتخاب کنید",Value = ""}
            };
            list.AddRange(_productService.GetSubGroupsForAddBook(id));
            return Json(new SelectList(list, "Value", "Text"));
        }

        #endregion

    }
}
