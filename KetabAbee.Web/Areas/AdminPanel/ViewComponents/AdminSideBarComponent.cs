using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KetabAbee.Application.Const;
using KetabAbee.Application.Extensions;
using KetabAbee.Application.Interfaces.Permission;
using Microsoft.AspNetCore.Mvc;

namespace KetabAbee.Web.Areas.AdminPanel.ViewComponents
{
    public class AdminSideBarComponent : ViewComponent
    {
        private readonly IPermissionService _permissionService;

        public AdminSideBarComponent(IPermissionService permissionService)
        {
            _permissionService = permissionService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var userId = HttpContext.User.GetUserId();

            #region permissios

            ViewBag.AdminTask = await _permissionService.IsUserHaveRole(userId, RoleIds.Manager);
            ViewBag.AdminRole = _permissionService.CheckPermission(PerIds.AdminRoles, userId);
            ViewBag.AdminUser = _permissionService.CheckPermission(PerIds.AdminUsers, userId);
            ViewBag.AdminTicket = _permissionService.CheckPermission(PerIds.AdminTickets, userId);
            ViewBag.AdminGroup = _permissionService.CheckPermission(PerIds.AdminGroups, userId);
            ViewBag.AdminBook = _permissionService.CheckPermission(PerIds.AdminBooks, userId);
            ViewBag.AdminOrder = _permissionService.CheckPermission(PerIds.AdminOrders, userId);
            ViewBag.AdminNews = _permissionService.CheckPermission(PerIds.AdminNews, userId);
            ViewBag.AdminContactUs = _permissionService.CheckPermission(PerIds.AdminContactUses, userId);
            ViewBag.AdminDiscount = _permissionService.CheckPermission(PerIds.AdminDiscounts, userId);
            ViewBag.AdminBlog = _permissionService.CheckPermission(PerIds.AdminBlogs, userId);
            ViewBag.AdminComment = _permissionService.CheckPermission(PerIds.AdminComments, userId);
            ViewBag.AdminExam = _permissionService.CheckPermission(PerIds.AdminExams, userId);
            ViewBag.AdminBanner = _permissionService.CheckPermission(PerIds.AdminBanners, userId);
            ViewBag.AdminAudioBook = _permissionService.CheckPermission(PerIds.AdminAudioBooks, userId);

            #endregion

            return View("AdminSideBar");
        }
    }
}
