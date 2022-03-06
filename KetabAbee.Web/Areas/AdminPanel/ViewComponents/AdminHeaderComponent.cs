using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KetabAbee.Application.Interfaces.User;
using Microsoft.AspNetCore.Mvc;

namespace KetabAbee.Web.Areas.AdminPanel.ViewComponents
{
    public class AdminHeaderComponent : ViewComponent
    {
        private readonly IUserService _userService;

        public AdminHeaderComponent(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View("AdminHeader");
        }
    }
}
