using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using KetabAbee.Application.DTOs.Wallet;
using KetabAbee.Application.Extensions;
using KetabAbee.Application.Interfaces.Order;
using KetabAbee.Application.Interfaces.User;
using KetabAbee.Application.Interfaces.Wallet;
using KetabAbee.Domain.Models.Order;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;

namespace KetabAbee.Web.Areas.UserPanel.Controllers
{
    public class OrderController : UserPanelBaseController
    {
        #region cunstroctor

        private readonly IOrderService _orderService;
        private readonly IUserService _userService;
        private readonly IPaymentService _paymentService;

        public OrderController(IOrderService orderService, IUserService userService, IPaymentService paymentService)
        {
            _orderService = orderService;
            _userService = userService;
            _paymentService = paymentService;
        }

        #endregion

        #region orders list

        [HttpGet("UserPanel/Orders")]
        public IActionResult Orders()
        {
            var userId = User.GetUserId();
            return View(_orderService.GetUserOrders(userId).ToList());
        }

        #endregion

        #region Order info

        [HttpGet("UserPanel/Order/{orderId}")]
        public IActionResult Order(int orderId)
        {
            var userId = User.GetUserId();
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
        public async Task<IActionResult> Cart(int orderId)
        {
            var userId = User.GetUserId();
            var order = _orderService.GetOrderForShowToUser(userId, orderId);
            ViewBag.UserWalleteBalance = await _userService.GetUserWalletBalance(userId);

            if (order == null)
            {
                return NotFound();
            }
            var userAddress = _userService.GetUserAddressByUserId(User.GetUserId());
            ViewBag.userAddress = userAddress;

            return View(order);
        }

        #endregion

        #region remove order detail from order

        [HttpGet("/Cart/Remove/{detailId}")]
        public IActionResult RemoveItemFromOrderDetail(int detailId, int orderId)
        {
            if (_orderService.RemoveItemOfOrderDetail(User.GetUserId(), orderId, detailId))
            {
                TempData["SuccessSwal"] = "از سبد خریدتان حذف شد";
                return Redirect($"/Cart/{orderId}");
            }

            TempData["ErrorSwal"] = "از سبد خریدتان حذف نشد";
            return Redirect($"/Cart/{orderId}");
        }

        [HttpGet("/RemoveFromCart/Item/{detailId}")]
        public IActionResult RemoveItemFromOrderDetailMobile(int detailId, int orderId, string url)
        {
            if (_orderService.RemoveItemOfOrderDetail(User.GetUserId(), orderId, detailId))
            {
                TempData["SuccessSwal"] = "از سبد خریدتان حذف شد";
                return Redirect(url);
            }

            TempData["ErrorSwal"] = "از سبد خریدتان حذف نشد";
            return Redirect(url);
        }

        #endregion

        #region change product count

        [Route("/Cart/ChangeCount")]
        public IActionResult SetProductCount(int productCount, string reqUrl, int orderId, int detailId)
        {
            var res = _orderService.UpdateDetailCount(User.GetUserId(), orderId, detailId, productCount);
            switch (res)
            {
                case ChangeCountResult.Success:
                    return Redirect(reqUrl);

                case ChangeCountResult.NotFound:
                    return BadRequest();

                case ChangeCountResult.OutOfRange:
                    TempData["WarningSwal"] = "موجودی کالا کافی نیست";
                    return Redirect(reqUrl);

                case ChangeCountResult.UnHandledException:
                default:
                    TempData["ErrorSwal"] = "خطایی در انجام عملیات رخ داد";
                    return Redirect(reqUrl);
            }
        }

        #endregion

        #region Pay

        [HttpGet("Pay/{orderId}")]
        public IActionResult Pay(int orderId, string address)
        {
            var userAddress = _userService.GetUserAddressByUserId(User.GetUserId());
            if (string.IsNullOrEmpty(userAddress))
            {
                if (!string.IsNullOrEmpty(address))
                {
                    var cart = _orderService.GetOrderForShowToUser(User.GetUserId(), orderId);

                    var callbackUrl = CommonExtensions.DomainAddress + Url.RouteUrl("ZarinPalPaymentResult");
                    var redirectUrl = "";
                    var status = _paymentService.CreatePaymentRequest(null, cart.OrderSum,
                        $"پرداخت هزینه سفارش {orderId }", callbackUrl, ref redirectUrl, User.GetUserEmail());

                    if (status == PaymentStatus.St100)
                    {
                        return Redirect(redirectUrl);
                    }

                    TempData["ErrorSwal"] = "پرداخت با شکست مواجه شد";
                    return Redirect($"/Cart/{orderId}");
                }

                TempData["ErrorSwal"] = "برای ثبت سفارش می بایست آدرس ارسال را تعیین کنید";
                return Redirect($"/Cart/{orderId}");
            }

            if (_orderService.PayByOrderId(User.GetUserId(), orderId))
            {
                _orderService.AddOrderAddress(orderId, User.GetUserId(), userAddress);
                TempData["SuccessSwal"] = "پرداخت با موفقیت انجام شد";
                return Redirect("/UserPanel/Dashboard");
            }

            TempData["ErrorSwal"] = "پرداخت با شکست مواجه شد";
            return Redirect($"/Cart/{orderId}");
        }

        #region zarin pal call back

        [AllowAnonymous]
        [HttpGet("zarin-pay-result", Name = "ZarinPalPaymentResult")]
        public async Task<IActionResult> CallBackZarinPal()
        {
            var authority = _paymentService.GetAuthorityCodeFromCallback(HttpContext);
            if (authority == "")
            {
                TempData["ErrorSwal"] = "پرداخت با شکست مواجه شد";
                return View();
            }
            var cart = await _orderService.GetUserUnFinalOrder(User.GetUserId());
            long refId = 0;
            var res = _paymentService.PaymentVerification(null, authority, cart.OrderSum, ref refId);

            if (res == PaymentStatus.St100)
            {
                if (_orderService.PayByOrderId(User.GetUserId(), cart.OrderId))
                {
                    TempData["SuccessSwal"] = "پرداخت موفق";
                    ViewBag.refId = refId;
                    return View();
                }
                TempData["ErrorSwal"] = "پرداخت ناموفق";
                return View();
            }

            TempData["ErrorSwal"] = "پرداخت ناموفق";
            return View();
        }

        #endregion

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult AddAddress(int orderId, string address)
        {
            if (!string.IsNullOrEmpty(address))
            {
                if (_orderService.AddOrderAddress(orderId, User.GetUserId(), address))
                {
                    TempData["SuccessSwal"] = "آدرس برای سفارش شما ثبت شد";
                    return Redirect($"/Cart/{orderId}");
                }

                TempData["ErrorSwal"] = "مشکلی هنگام ثبت آدرس رخ داد";
                return Redirect($"/Cart/{orderId}");
            }

            TempData["ErrorSwal"] = "برای ثبت سفارش باید فیلد آدرس را پر کنید";
            return Redirect($"/Cart/{orderId}");
        }

        #endregion
    }
}
