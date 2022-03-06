using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KetabAbee.Application.Extensions;
using KetabAbee.Application.Interfaces.Task;
using KetabAbee.Application.Interfaces.User;
using Microsoft.AspNetCore.Mvc;

namespace KetabAbee.Web.Areas.AdminPanel.ViewComponents
{
    public class AdminHeaderComponent : ViewComponent
    {
        private readonly IUserService _userService;
        private readonly ITaskService _taskService;

        public AdminHeaderComponent(IUserService userService, ITaskService taskService)
        {
            _userService = userService;
            _taskService = taskService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var userId = HttpContext.User.GetUserId();

            ViewBag.AvatarName = await _userService.GetAvatarNameByUserId(userId);

            var roleIds = await _userService.GetUserRoleIds(userId);
            ViewBag.CurrentAdminTasks = _taskService.GetTasksForAdmin(roleIds);

            return View("AdminHeader");
        }
    }
}
