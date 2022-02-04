using KetabAbee.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GoogleReCaptcha.V3.Interface;
using KetabAbee.Application.DTOs;
using KetabAbee.Application.DTOs.Book;
using KetabAbee.Application.DTOs.Contact;
using KetabAbee.Application.Extensions;
using KetabAbee.Application.Interfaces.Contact;
using KetabAbee.Application.Interfaces.Product;
using KetabAbee.Application.Interfaces.User;
using KetabAbee.Application.Interfaces.Wallet;
using KetabAbee.Domain.Models.ContactUs;
using Microsoft.AspNetCore.Http;

namespace KetabAbee.Web.Controllers
{
    public class HomeController : Controller
    {
        #region constructor

        private readonly IProductService _productService;
        private readonly IContactService _contactService;
        private readonly ICaptchaValidator _captchaValidator;

        public HomeController(IProductService productService, IContactService contactService, ICaptchaValidator captchaValidator)
        {
            _productService = productService;
            _contactService = contactService;
            _captchaValidator = captchaValidator;
        }

        #endregion

        #region index

        public IActionResult Index()
        {
            return View();
        }

        #endregion

        #region Online Pament

        //[HttpGet("Wallet/OnlinePayment/{id}")]
        //public IActionResult OnlinePayment(int id) // wallet Id
        //{
        //    if (HttpContext.Request.Query["Status"] != "" &&
        //        HttpContext.Request.Query["Status"].ToString().ToLower() == "ok" &&
        //        HttpContext.Request.Query["Authority"] != "")
        //    {
        //        string auth = HttpContext.Request.Query["Authority"];

        //        var wallet = _walletService.GetWalletById(id);
        //        var payment = new ZarinpalSandbox.Payment((int) wallet.Amount);
        //        var res = payment.Verification(auth).Result;

        //        if (res.Status == 100)
        //        {
        //            ViewBag.Code = res.RefId;
        //            ViewBag.IsSuccess = true;
        //            wallet.IsPay = true;
        //            _walletService.UpdateWallet(wallet);
        //        }
        //    }

        //    return View();
        //}

        #endregion

        #region how to shop

        [HttpGet("Purchase/Guide")]
        public IActionResult HowToShop()
        {
            return View();
        }

        #endregion

        #region add email to news letter

        [HttpPost("AddToNLE")]
        public IActionResult AddEmailToNewsLetterEmails(string email, string url)
        {
            if (!string.IsNullOrEmpty(email))
            {
                if (_contactService.EmailInNewsEmailsIsUnique(email))
                {
                    if (_contactService.AddEmailToNewsEmails(email))
                    {
                        TempData["SuccessMessage"] = "با موفقیت به خبرنامه افزوده شد";
                        return Redirect(url);
                    }
                    TempData["ErrorMessage"] = "عملیات افزودن به خبرنامه با مشکل مواجه شد";
                    return Redirect(url);
                }
                TempData["InfoMessage"] = "این ایمیل قبلا ثبت شده است";
                return Redirect(url);
            }
            TempData["ErrorMessage"] = "ایمیل را وارد کنید";
            return Redirect(url);
        }

        #endregion

        #region About us

        [HttpGet("Page/About")]
        public IActionResult AboutUs()
        {
            return View();
        }

        #endregion

        #region About us

        [HttpGet("Page/Contact")]
        public IActionResult ContactUs()
        {
            return View();
        }

        [HttpPost("Page/Contact"), ValidateAntiForgeryToken]
        public async Task<IActionResult> ContactUs(CreateContactUsViewModel contactUs)
        {
            if (!await _captchaValidator.IsCaptchaPassedAsync(contactUs.Captcha))
            {
                TempData["ErrorMessage"] = "احراز هویت کپچا انجام نشد چند لحظه دیگر تلاش کنید";
                return View(contactUs);
            }
            if (!ModelState.IsValid)
            {
                return View(contactUs);
            }

            if (User.Identity.IsAuthenticated)
            {
                if (_contactService.CreateContactUs(contactUs, HttpContext.GetUserIp(), User.GetUserId()))
                {
                    TempData["SuccessMessage"] = "پیام شما با موفقیت ارسال شد";
                    return RedirectToAction("Index");
                }
                TempData["ErrorMessage"] = "خطایی در ارسال پیام رخ داده است";
                return RedirectToAction("Index");
            }

            if (_contactService.CreateContactUs(contactUs, HttpContext.GetUserIp(), null))
            {
                TempData["SuccessMessage"] = "پیام شما با موفقیت ارسال شد";
                return RedirectToAction("Index");
            }
            TempData["ErrorMessage"] = "خطایی در ارسال پیام رخ داده است";
            return RedirectToAction("Index");
        }

        #endregion

        #region CKEditor Images

        [HttpPost]
        [Route("file-upload")]
        public IActionResult UploadImage(IFormFile upload, string CKEditorFuncNum, string CKEditor, string langCode)
        {
            if (upload.Length <= 0) return null;

            var fileName = Guid.NewGuid() + Path.GetExtension(upload.FileName).ToLower();

            var path = Path.Combine(
                Directory.GetCurrentDirectory(), "wwwroot/MyImages",
                fileName);

            using (var stream = new FileStream(path, FileMode.Create))
            {
                upload.CopyTo(stream);

            }

            var url = $"{"/MyImages/"}{fileName}";


            return Json(new { uploaded = true, url });
        }

        #endregion

    }
}
