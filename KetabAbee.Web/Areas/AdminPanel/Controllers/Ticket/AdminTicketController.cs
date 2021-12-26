using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KetabAbee.Application.DTOs.Ticket;
using KetabAbee.Application.Interfaces.Ticket;
using KetabAbee.Application.Interfaces.User;
using KetabAbee.Domain.Models.Ticket;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace KetabAbee.Web.Areas.AdminPanel.Controllers.Ticket
{
    public class AdminTicketController : AdminBaseController
    {
        #region constructure

        private readonly ITicketService _ticketService;

        public AdminTicketController(ITicketService ticketService)
        {
            _ticketService = ticketService;
        }

        #endregion

        #region Tickets List

        [HttpGet("Admin/Tickets")]
        public IActionResult Index(FilterTicketViewModel filter)
        {
            return View(_ticketService.FilterTickets(filter));
        }

        #endregion

    }
}
