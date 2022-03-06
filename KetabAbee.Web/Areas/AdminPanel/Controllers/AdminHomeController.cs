using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KetabAbee.Application.DTOs.Admin.Task;
using KetabAbee.Application.Extensions;
using KetabAbee.Application.Interfaces.Order;
using KetabAbee.Application.Interfaces.Product;
using KetabAbee.Application.Interfaces.Task;
using KetabAbee.Application.Interfaces.Ticket;
using KetabAbee.Application.Interfaces.User;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace KetabAbee.Web.Areas.AdminPanel.Controllers
{
    public class AdminHomeController : AdminBaseController
    {
        #region constructor

        private readonly IProductService _productService;
        private readonly IUserService _userService;
        private readonly ITicketService _ticketService;
        private readonly IOrderService _orderService;
        private readonly ITaskService _taskService;

        public AdminHomeController(IProductService productService, IUserService userService, ITicketService ticketService, IOrderService orderService, ITaskService taskService)
        {
            _productService = productService;
            _userService = userService;
            _ticketService = ticketService;
            _orderService = orderService;
            _taskService = taskService;
        }

        #endregion

        #region Index

        [HttpGet("Admin")]
        public async Task<IActionResult> Home()
        {
            #region profile box

            ViewBag.Profile = _userService.GetInfoByUserEmail(User.GetUserEmail());

            #endregion

            #region last users box

            var lastMonthUsers = _userService.GetLastNDaysUsers(30).Count();
            ViewBag.LastMonthUsers = lastMonthUsers;
            var lastWeekUsers = _userService.GetLastNDaysUsers(7).Count();
            ViewBag.LastWeekUsers = lastWeekUsers;
            var lastDayUsers = _userService.GetLastNDaysUsers(1).Count();
            ViewBag.LastDayUsers = lastDayUsers;

            #endregion

            #region user statistics

            ViewBag.UserStatics = _userService.GetUsersStaticsForAdmin();

            #endregion

            #region ticket statistics

            ViewBag.TicketStatics = _ticketService.GetTicketStaticInfo();

            #endregion

            #region sell statics

            ViewBag.SellStatics = _orderService.GetSellInfo();

            #endregion

            #region tasks

            var roleIds = await _userService.GetUserRoleIds(User.GetUserId());
            ViewBag.CurrentAdminTasks = _taskService.GetTasksForAdmin(roleIds);

            #endregion

            return View();
        }

        #endregion

        #region Json

        public async Task<IActionResult> GetSubGroups(int id) // id = groupId
        {
            var list = new List<SelectListItem>
            {
                new(){Text = "دسته بندی فرعی اول را انتخاب کنید",Value = ""}
            };
            list.AddRange(await _productService.GetSubGroupsForAddBook(id));
            return Json(new SelectList(list, "Value", "Text"));
        }

        public async Task<IActionResult> GetSubGroups2(int id) // id = sub id 1
        {
            var list = new List<SelectListItem>
            {
                new(){Text = "دسته بندی فرعی دوم را انتخاب کنید",Value = ""}
            };
            list.AddRange(await _productService.GetSubGroupsForAddBook(id));
            return Json(new SelectList(list, "Value", "Text"));
        }

        #endregion

        #region change task is completed

        [HttpPost("Change/T/{taskId}")]
        public IActionResult ChangeTaskIsCompleted(int taskId)
        {
            if (_taskService.ChangeTaskIsCompleted(taskId))
            {
                TempData["SuccessMessage"] = "انجام شد";
                return RedirectToAction("Home");
            }

            TempData["ErrorMessage"] = "مشکلی در انجام عملیات رخ داد";
            return RedirectToAction("Home");
        }

        #endregion
    }
}
