using Microsoft.AspNetCore.Mvc;
using KetabAbee.Application.DTOs.Ticket;
using KetabAbee.Application.Interfaces.Ticket;
using KetabAbee.Domain.Models.Ticket;


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

        #region Delete Ticket

        [HttpGet("Admin/Tickets/DeleteTicket/{id}")]
        public IActionResult DeleteTicket(int id)
        {
            if (_ticketService.DeleteTicketById(id))
            {
                TempData["SuccessMessage"] = "حذف تیکت با موفقیت انجام شد";
                return RedirectToAction("Index");
            }
            TempData["ErrorMessage"] = "عملیات حذف تیکت با شکست مواجه شد";
            return RedirectToAction("Index");
        }

        #endregion

        #region Show Ticket Details

        [HttpGet("Admin/Tickets/Details/{id}")]
        public IActionResult ShowTicketDetails(int id)
        {
            return View(_ticketService.GetTicketById(id));
        }

        #endregion

        #region Ticket = Is Read or Isnt Read

        [HttpGet("Admin/Tickets/IsRead/{id}")]
        public IActionResult TicketIsRead(int id)
        {
            if (_ticketService.TicketIsRead(id))
            {
                TempData["SuccessMessage"] = "حالت خوانده شده تیکت تغییر یافت";
                return RedirectToAction("Index");
            }
            TempData["ErrorMessage"] = "عملیات با شکست مواجه شد";
            return RedirectToAction("Index");
        }

        #endregion

        #region Answer Ticket

        [HttpGet("Admin/Tickets/Answer/{id}")]
        public IActionResult AnswerTicket(int id) // id = ticketId
        {
            return RedirectToAction("Index");
        }

        #endregion

    }
}
