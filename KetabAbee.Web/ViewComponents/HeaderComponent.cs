using System.Threading.Tasks;
using KetabAbee.Application.Const;
using KetabAbee.Application.Extensions;
using KetabAbee.Application.Interfaces.Order;
using KetabAbee.Application.Interfaces.Permission;
using KetabAbee.Application.Interfaces.Product;
using KetabAbee.Application.Interfaces.User;
using Microsoft.AspNetCore.Mvc;

namespace KetabAbee.Web.ViewComponents
{
    public class HeaderComponent : ViewComponent
    {
        private readonly IUserService _userService;
        private readonly IOrderService _orderService;
        private readonly IProductService _productService;
        private readonly IPermissionService _permissionService;

        public HeaderComponent(IUserService userService, IOrderService orderService, IProductService productService, IPermissionService permissionService)
        {
            _userService = userService;
            _orderService = orderService;
            _productService = productService;
            _permissionService = permissionService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            ViewBag.IsAuth = User.Identity.IsAuthenticated;
            if (!User.Identity.IsAuthenticated) return View("Header");

            var userId = HttpContext.User.GetUserId();
            ViewBag.IsAdmin = _permissionService.CheckPermission(PerIds.AdminPanel, HttpContext.User.GetUserEmail());
            ViewBag.UserBookIdsCount = await _userService.GetUserFavBookCount(userId);
            ViewBag.UserCurrentOrder = await _orderService.GetUserUnFinalOrder(userId);
            ViewBag.AvatarName = await _userService.GetAvatarNameByUserId(userId);
            ViewBag.UserWalletBalance = await _userService.GetUserWalletBalance(userId);

            return View("Header");
        }
    }
}
