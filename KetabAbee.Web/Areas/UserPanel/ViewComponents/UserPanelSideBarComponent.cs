using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KetabAbee.Application.Extensions;
using KetabAbee.Application.Interfaces.Ticket;
using KetabAbee.Application.Interfaces.User;
using Microsoft.AspNetCore.Mvc;

namespace KetabAbee.Web.Areas.UserPanel.ViewComponents
{
    public class UserPanelSideBarComponent : ViewComponent
    {
        private readonly IUserService _userService;
        private readonly ITicketService _ticketService;

        public UserPanelSideBarComponent(IUserService userService, ITicketService ticketService)
        {
            _userService = userService;
            _ticketService = ticketService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            ViewBag.HaveUnreadTicket = _ticketService.UserHaveUnReadTicket(HttpContext.User.GetUserId());
            return await Task.FromResult((IViewComponentResult)View("UserPanelSideBar", _userService.GetUserPanelSideBarInfoByEmail(User.Identity.Name)));
        }
    }
}
