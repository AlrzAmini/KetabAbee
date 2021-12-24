using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using KetabAbee.Application.DTOs.Ticket;
using KetabAbee.Application.Extensions;
using KetabAbee.Application.Interfaces.Ticket;

namespace KetabAbee.Web.Areas.UserPanel.Controllers
{
    public class TicketController : UserPanelBaseController
    {
        #region constructor

        private readonly ITicketService _ticketService;

        public TicketController(ITicketService ticketService)
        {
            _ticketService = ticketService;
        }

        #endregion

        #region List tickets

        [HttpGet("Tickets")]
        public IActionResult Index(FilterTicketViewModel filter)
        {
            filter.UserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            ViewBag.orderby = filter.OrderBy.GetEnumName();
            
            return View(_ticketService.FilterTickets(filter));
        }

        #endregion

        #region Add ticket

        [HttpGet("Ticket/AddTicket")]
        public IActionResult AddTicket()
        {
            return View();
        }

        [HttpPost("Ticket/AddTicket"), ValidateAntiForgeryToken]
        public IActionResult AddTicket(AddTicketViewmodel ticket)
        {
            if (!ModelState.IsValid)
            {
                return View(ticket);
            }

            bool res = _ticketService.AddTicket(ticket, int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)));

            if (res)
            {
                TempData["SuccessMessage"] = "ثبت تیکت با موفقیت انجام شد";
                TempData["InfoMessage"] = "پاسخ به تیکت شما پس از بررسی ارسال خواهد شد";
                return RedirectToAction("Index");
            }

            TempData["ErrorMessage"] = "ثبت تیکت با شکست مواجه شد";
            return View(ticket);
        }

        #endregion

        #region Show ticket

        [HttpGet("Ticket/ShowTicket/{id}")]
        public IActionResult ShowTicket(int id) // id = ticket Id
        {
            var ticket = _ticketService.GetTicketById(id);

            if (ticket == null)
            {
                return NotFound();
            }
            
            return View(ticket);
        }

        #endregion
    }
}
