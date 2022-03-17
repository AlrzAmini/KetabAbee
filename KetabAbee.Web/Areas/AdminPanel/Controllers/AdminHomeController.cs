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
using KetabAbee.Domain.Models.User;
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

            ViewBag.LastMonthUsers = await _userService.GetLastNDaysUsersCount(30);
            ViewBag.LastWeekUsers = await _userService.GetLastNDaysUsersCount(7);
            ViewBag.LastDayUsers = await _userService.GetLastNDaysUsersCount(1);

            #endregion

            #region user statistics

            ViewBag.UserStatics = await _userService.GetUsersStaticsForAdmin();

            #endregion

            #region ticket statistics

            ViewBag.TicketStatics = await _ticketService.GetTicketStaticInfo();

            #endregion

            #region sell statics

            ViewBag.SellStatics = await _orderService.GetSellInfo();

            #endregion

            #region tasks

            var roleIds = await _userService.GetUserRoleIds(User.GetUserId());
            ViewBag.CurrentAdminTasks = await _taskService.GetTasksForAdmin(roleIds);

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

        #region ban ip

        [HttpPost("Ban/Ip")]
        public async Task<IActionResult> BanUserIp(string userIp)
        {
            var res = await _userService.BanUserByIp(userIp);
            switch (res)
            {
                case BanIpResult.Success:
                    TempData["SuccessMessage"] = "آی پی وارد شده بن شد";
                    return RedirectToAction("Home");
                case BanIpResult.Error:
                    TempData["ErrorMessage"] = "مشکلی در انجام عملیات رخ داد";
                    return RedirectToAction("Home");
                case BanIpResult.IpIsAlreadyExist:
                    TempData["WarningMessage"] = "این آی پی قبلا بن شده است";
                    return RedirectToAction("Home");
                default:
                    TempData["ErrorMessage"] = "مشکلی در انجام عملیات رخ داد";
                    return RedirectToAction("Home");
            }
        }

        #endregion
    }
}
