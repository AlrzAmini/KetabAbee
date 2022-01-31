using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KetabAbee.Application.Const;
using KetabAbee.Application.DTOs.Admin.Order;
using KetabAbee.Application.Interfaces.Order;
using KetabAbee.Application.Security;

namespace KetabAbee.Web.Areas.AdminPanel.Controllers.Orders
{
    [PermissionChecker(PerIds.AdminOrders)]
    public class AdminOrderController : AdminBaseController
    {
        #region constructor

        private readonly IOrderService _orderService;

        public AdminOrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        #endregion

        #region Finalled Order List

        [HttpGet("Admin/Orders")]
        public IActionResult Index(OrdersForShowInAdminViewModel order)
        {
            return View(_orderService.GetPayedOrdersForAdmin(order));
        }

        #endregion

        #region change order to is completed

        [HttpGet("Change/{orderId}")]
        public IActionResult ChangeIsCompleted(int orderId)
        {
            if (_orderService.ChangeIsCompleted(orderId))
            {
                TempData["SuccessMessage"] = "وضعیت ارسال با موفقیت ویرایش شد";
                return RedirectToAction("Index");
            }
            TempData["ErrorMessage"] = "وضعیت ارسال ویرایش نشد";
            return RedirectToAction("Index");
        }

        #endregion

        #region Order info

        [HttpGet("Order/Info/{orderId}")]
        public IActionResult OrderInfo(int orderId)
        {
            return View(_orderService.GetOrderByIdForShowInfo(orderId));
        }

        #endregion
    }
}
