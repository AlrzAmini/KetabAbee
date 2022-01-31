using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KetabAbee.Application.DTOs.Admin.Order;
using KetabAbee.Application.Interfaces.Order;

namespace KetabAbee.Web.Areas.AdminPanel.Controllers.Orders
{
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

        //TODO

        #endregion
    }
}
