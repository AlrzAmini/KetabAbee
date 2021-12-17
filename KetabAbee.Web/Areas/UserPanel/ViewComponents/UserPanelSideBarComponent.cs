using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KetabAbee.Application.Interfaces.User;
using Microsoft.AspNetCore.Mvc;

namespace KetabAbee.Web.Areas.UserPanel.ViewComponents
{
    public class UserPanelSideBarComponent : ViewComponent
    {
        private readonly IUserService _userService;

        public UserPanelSideBarComponent(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return await Task.FromResult((IViewComponentResult)View("UserPanelSideBar", _userService.GetUserPanelSideBarInfoByEmail(User.Identity.Name)));
        }
    }
}
