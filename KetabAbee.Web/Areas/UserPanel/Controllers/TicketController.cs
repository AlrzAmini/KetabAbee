using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Ganss.XSS;
using GoogleReCaptcha.V3.Interface;
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
        private readonly ICaptchaValidator _captchaValidator;

        public TicketController(ITicketService ticketService, ICaptchaValidator captchaValidator)
        {
            _ticketService = ticketService;
            _captchaValidator = captchaValidator;
        }

        #endregion

        #region List tickets

        [HttpGet("Tickets")]
        public IActionResult Index(FilterTicketViewModel filter)
        {
            filter.UserId = User.GetUserId();

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
        public async Task<IActionResult> AddTicket(AddTicketViewmodel ticket)
        {
            if (!await _captchaValidator.IsCaptchaPassedAsync(ticket.Captcha))
            {
                TempData["ErrorSwal"] = "احراز هویت کپچا انجام نشد چند لحظه دیگر تلاش کنید";
                return View(ticket);
            }

            if (!ModelState.IsValid)
            {
                return View(ticket);
            }

            if (_ticketService.AddTicket(ticket, User.GetUserId()))
            {
                TempData["SuccessSwal"] = "ثبت تیکت با موفقیت انجام شد";
                TempData["InfoMessage"] = "پاسخ به تیکت شما پس از بررسی ارسال خواهد شد";
                return RedirectToAction("Index");
            }

            TempData["ErrorSwal"] = "ثبت تیکت با شکست مواجه شد";
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
                TempData["SuccessSwal"] = "حالت خوانده شده تیکت تغییر یافت";
                return RedirectToAction("Index");
            }
            TempData["ErrorSwal"] = "عملیات با شکست مواجه شد";
            return RedirectToAction("Index");
        }


        #endregion

        #region Answer Ticket

        public async Task<IActionResult> CreateAnswer(CreateTicketAnswerViewModel answer)
        {
            var id = answer.TicketId;
            if (!await _captchaValidator.IsCaptchaPassedAsync(answer.Captcha))
            {
                TempData["ErrorSwal"] = "احراز هویت کپچا انجام نشد چند لحظه دیگر تلاش کنید";
                return RedirectToAction("ShowTicket", new { id });
            }

            if (!string.IsNullOrEmpty(answer.AnswerBody))
            {
                answer.SenderId = User.GetUserId();

                if (_ticketService.AddAnswerFromUser(answer))
                {
                    TempData["SuccessSwal"] = "پاسخ تیکت ثبت شد";
                    return RedirectToAction("ShowTicket", new { id });
                }

                TempData["ErrorSwal"] = "پاسخ تیکت ثبت نشد";
                return RedirectToAction("ShowTicket", new { id });
            }
            TempData["ErrorSwal"] = "متن پاسخ نمی تواند خالی باشد";
            return RedirectToAction("ShowTicket", new { id });
        }

        #endregion
    }
}
