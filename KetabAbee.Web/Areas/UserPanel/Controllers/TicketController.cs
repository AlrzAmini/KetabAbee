using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Ganss.XSS;
using KetabAbee.Application.DTOs.Ticket;
using KetabAbee.Application.Extensions;
using KetabAbee.Application.Interfaces.Ticket;
using KetabAbee.Domain.Models.Ticket;

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

            // sanitize body
            var sanitizer = new HtmlSanitizer();
            ticket.Body = sanitizer.Sanitize(ticket.Body);

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

        public IActionResult ShowTicket(int id) // id = ticket Id
        {
            var ticketWithAnswers = _ticketService.GetTicketForShowTicketInAdmin(id);

            if (ticketWithAnswers == null || ticketWithAnswers.Ticket.SenderId != User.GetUserId())
            {
                return NotFound();
            }
            
            return View(ticketWithAnswers);
        }

        #endregion

        #region is read

        [HttpGet("Ticket/IsReadBySender/{id}")]
        public IActionResult TicketIsReadBySender(int id) //id = ticketId
        {
            if (_ticketService.TicketIsReadBySender(id))
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
                var answer = new TicketAnswer
                {
                    TicketId = id,
                    SenderId = User.GetUserId(),
                    AnswerBody = answerBody,
                    Ticket = _ticketService.GetTicketById(id)
                };

                if (_ticketService.AddAnswerFromUser(answer))
                {
                    TempData["SuccessMessage"] = "پاسخ تیکت ثبت شد";
                    return RedirectToAction("ShowTicket", new { id });
                }

                TempData["ErrorMessage"] = "پاسخ تیکت ثبت نشد";
                return RedirectToAction("ShowTicket", new { id });
            }
            TempData["ErrorMessage"] = "متن پاسخ نمی تواند خالی باشد";
            return RedirectToAction("ShowTicket", new { id });
        }

        #endregion
    }
}
