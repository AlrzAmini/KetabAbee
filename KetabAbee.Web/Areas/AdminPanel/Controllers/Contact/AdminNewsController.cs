using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KetabAbee.Application.DTOs.Admin;
using KetabAbee.Application.Interfaces.Contact;

namespace KetabAbee.Web.Areas.AdminPanel.Controllers.Contact
{
    [Route("Admin/News")]
    public class AdminNewsController : AdminBaseController
    {
        #region constructor

        private readonly IContactService _contactService;

        public AdminNewsController(IContactService contactService)
        {
            _contactService = contactService;
        }

        #endregion

        #region index

        [HttpGet]
        public IActionResult Index()
        {
            return View(_contactService.GetNewsLetters().ToList());
        }

        #endregion

        #region Add News Letter

        [HttpGet("Add/NewsLetter")]
        public IActionResult AddNewsLetter()
        {
            return View();
        }

        [HttpPost("Add/NewsLetter"),ValidateAntiForgeryToken]
        public IActionResult AddNewsLetter(CreateNewsLetterViewModel news)
        {
            if (!ModelState.IsValid)
            {
                return View(news);
            }

            if (_contactService.AddNewsLetter(news))
            {
                TempData["SuccessMessage"] = "خبرنامه اضافه شد";
                return RedirectToAction("Index");
            }

            TempData["ErrorMessage"] = "خبرنامه اضافه نشد";
            return RedirectToAction("Index");
        }

        #endregion

        #region Send to all

        [HttpGet("Send/ToAll")]
        public IActionResult SendToAll(int newsId)
        {
            if (_contactService.SendNewsLetterToAll(newsId))
            {
                TempData["SuccessMessage"] = "به تمامی ایمیل های موجود در خبرنامه ارسال شد";
                return RedirectToAction("Index");
            }
            TempData["ErrorMessage"] = "خطایی در ارسال ایمیل ها رخ داد";
            return RedirectToAction("Index");
        }

        #endregion

        #region emails list

        [HttpGet("Submit/Done/Emails")]
        public IActionResult Emails()
        {
            return View(_contactService.GetNewsLetterEmails().ToList());
        }

        #endregion
    }
}
