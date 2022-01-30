using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using KetabAbee.Application.Extensions;
using KetabAbee.Application.Interfaces.Order;

namespace KetabAbee.Web.Areas.UserPanel.Controllers
{
    public class OrderController : UserPanelBaseController
    {
        #region cunstroctor

        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        #endregion

        #region orders list

        [HttpGet("UserPanel/Orders")]
        public IActionResult Orders()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            return View(_orderService.GetUserOrders(userId).ToList());
        }

        #endregion

        #region Order info

        [HttpGet("UserPanel/Order/{orderId}")]
        public IActionResult Order(int orderId)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var order = _orderService.GetOrderForShowToUser(userId, orderId);

            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        #endregion

        #region Cart

        [HttpGet("Cart/{orderId}")]
        public IActionResult Cart(int orderId)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var order = _orderService.GetOrderForShowToUser(userId, orderId);

            if (order == null)
            {
                return NotFound();
            }

            var reqUrl = Microsoft.AspNetCore.Http.Extensions.UriHelper.GetDisplayUrl(this.Request);
            ViewBag.reqUrl = reqUrl;
            return View(order);
        }

        #endregion

        #region remove order detail from order

        [HttpGet("/Cart/Remove/{detailId}")]
        public IActionResult RemoveItemFromOrderDetail(int detailId, int orderId)
        {
            if (_orderService.RemoveItemOfOrderDetail(User.GetUserId(), orderId, detailId))
            {
                TempData["SuccessMessage"] = "از سبد خریدتان حذف شد";
                return Redirect($"/Cart/{orderId}");
            }

            TempData["ErrorMessage"] = "از سبد خریدتان حذف نشد";
            return Redirect($"/Cart/{orderId}");
        }

        #endregion

        #region change product count

        [Route("/Cart/ChangeCount")]
        public IActionResult SetProductCount(int productCount, string reqUrl, int orderId, int detailId)
        {
            //TODO : Check service with inventory
            if (_orderService.UpdateDetailCount(User.GetUserId(), orderId, detailId, productCount))
            {
                return Redirect(reqUrl);
            }

            return NotFound();
        }

        #endregion

        #region Pay

        [HttpGet("Pay/{orderId}")]
        public IActionResult Pay(int orderId)
        {

        }

        #endregion
    }
}
