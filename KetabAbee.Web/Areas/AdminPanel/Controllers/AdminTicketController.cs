using System;
using Microsoft.AspNetCore.Mvc;
using KetabAbee.Application.DTOs.Ticket;
using KetabAbee.Application.Extensions;
using KetabAbee.Application.Interfaces.Ticket;
using KetabAbee.Domain.Models.Ticket;


namespace KetabAbee.Web.Areas.AdminPanel.Controllers
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

        public IActionResult ShowTicketDetails(int id)
        {
            return View(_ticketService.GetTicketForShowTicketInAdmin(id));
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

        public IActionResult CreateAnswer(int id, string answerBody)
        {
            if (!string.IsNullOrEmpty(answerBody))
            {
                var answer = new TicketAnswer()
                {
                    TicketId = id,
                    SenderId = User.GetUserId(),
                    AnswerBody = answerBody,
                    SendDate = DateTime.Now
                };

                if (_ticketService.AddAnswer(answer))
                {
                    TempData["SuccessMessage"] = "پاسخ تیکت ثبت شد";
                    return RedirectToAction("ShowTicketDetails", new { id });
                }

                TempData["ErrorMessage"] = "پاسخ تیکت ثبت نشد";
                return RedirectToAction("ShowTicketDetails", new { id });
            }
            TempData["ErrorMessage"] = "متن پاسخ نمی تواند خالی باشد";
            return RedirectToAction("ShowTicketDetails", new { id });
        }

        #endregion

    }
}
