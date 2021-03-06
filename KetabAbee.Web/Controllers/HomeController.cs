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
using KetabAbee.Application.Convertors;
using KetabAbee.Application.DTOs.Book;
using KetabAbee.Application.DTOs.Contact;
using KetabAbee.Application.DTOs.Wallet;
using KetabAbee.Application.Extensions;
using KetabAbee.Application.Interfaces.Contact;
using KetabAbee.Application.Interfaces.Product;
using KetabAbee.Application.Interfaces.Wallet;
using KetabAbee.Application.Senders;
using KetabAbee.Domain.Models.Products;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace KetabAbee.Web.Controllers
{
    public class HomeController : Controller
    {
        #region constructor

        private readonly IContactService _contactService;
        private readonly ICaptchaValidator _captchaValidator;
        private readonly IViewRenderService _renderService;
        private readonly IProductService _productService;
        private readonly IPaymentService _paymentService;

        public HomeController(IContactService contactService, ICaptchaValidator captchaValidator, IViewRenderService renderService, IProductService productService, IPaymentService paymentService)
        {
            _contactService = contactService;
            _captchaValidator = captchaValidator;
            _renderService = renderService;
            _productService = productService;
            _paymentService = paymentService;
        }

        #endregion

        #region index

        public IActionResult Index()
        {
            return View();
        }

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
        public async Task<IActionResult> AddEmailToNewsLetterEmails(string email, string url)
        {
            try
            {
                if (!string.IsNullOrEmpty(email))
                {
                    email = FixText.EmailFixer(email);
                    if (!email.IsValidEmail())
                    {
                        TempData["WarningSwal"] = "ایمیل وارد شده صحیح نیست";
                        return Redirect(url);
                    }
                    if (_contactService.EmailInNewsEmailsIsUnique(email))
                    {
                        if (await _contactService.AddEmailToNewsEmails(email))
                        {
                            //send email
                            var body = _renderService.RenderToStringAsync("_JoinToNews", email);
                            await SendEmail.Send(email, "عضویت در خبرنامه کتاب آبی", body);
                            TempData["SuccessSwal"] = "با موفقیت به خبرنامه افزوده شد";

                            return Redirect(url);
                        }
                        TempData["ErrorSwal"] = "عملیات افزودن به خبرنامه با مشکل مواجه شد";
                        return Redirect(url);
                    }
                    TempData["WarningSwal"] = "این ایمیل قبلا ثبت شده است";
                    return Redirect(url);
                }
                TempData["WarningSwal"] = "ایمیل را وارد کنید";
                return Redirect(url);
            }
            catch
            {
                TempData["ErrorSwal"] = "مشکلی رخ داد";
                return Redirect(url);
            }
        }

        #endregion

        #region About us

        [HttpGet("Page/About")]
        public IActionResult AboutUs()
        {
            return View();
        }

        #endregion

        #region Contact us

        [HttpGet("load-contactus-modal")]
        public IActionResult LoadContactUsModal()
        {
            return PartialView("_ContactUsModal",null);
        }


        [HttpGet("Page/Contact")]
        public IActionResult ContactUs()
        {
            return View();
        }

        [HttpPost("Page/Contact"), ValidateAntiForgeryToken]
        public IActionResult ContactUs(CreateContactUsViewModel contactUs)
        {
            if (!ModelState.IsValid)
            {
                return View(contactUs);
            }

            if (User.Identity.IsAuthenticated)
            {
                if (_contactService.CreateContactUs(contactUs, HttpContext.GetUserIp(), User.GetUserId()))
                {
                    TempData["SuccessSwal"] = "پیام شما با موفقیت ارسال شد";
                    return RedirectToAction("Index");
                }
                TempData["ErrorSwal"] = "خطایی در ارسال پیام رخ داده است";
                return RedirectToAction("Index");
            }

            if (_contactService.CreateContactUs(contactUs, HttpContext.GetUserIp(), null))
            {
                TempData["SuccessSwal"] = "پیام شما با موفقیت ارسال شد";
                return RedirectToAction("Index");
            }
            TempData["ErrorSwal"] = "خطایی در ارسال پیام رخ داده است";
            return RedirectToAction("Index");
        }

        #endregion

        #region User benefits

        [HttpGet("Page/UserBenefits")]
        public IActionResult UserBenefits()
        {
            return View();
        }

        #endregion

        #region Request a branch

        [HttpGet("Form/Req/Branch")]
        public IActionResult RequestBranch()
        {
            return View();
        }

        [HttpPost("Form/Req/Branch"), ValidateAntiForgeryToken]
        public async Task<IActionResult> RequestBranch(CreateRequestBranchViewModel model)
        {
            if (!await _captchaValidator.IsCaptchaPassedAsync(model.Captcha))
            {
                TempData["ErrorSwal"] = "احراز هویت کپچا انجام نشد چند لحظه دیگر تلاش کنید";
                return View(model);
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (await _contactService.AddRequestBranch(model))
            {
                TempData["SuccessSwal"] = "درخواست شعبه با موفقیت ارسال شد";
                return RedirectToAction("Index");
            }
            TempData["ErrorSwal"] = "خطایی در ارسال درخواست رخ داده است";
            return RedirectToAction("Index");
        }

        #endregion

        #region 404 not found

        [HttpGet("404Error")]
        public IActionResult NotFoundedPage()
        {
            return View();
        }


        #endregion

        #region 500 error 

        [HttpGet("500Error")]
        public IActionResult Server500ErrorPage()
        {
            return View();
        }


        #endregion

        #region advanced search

        [HttpGet("Advanced-Search")]
        public async Task<IActionResult> AdvancedSearch(FilterAdvancedViewModel filter)
        {
            #region publishers

            var selectPublishers = new List<SelectListItem>
            {
                new(){Text = "ناشر ..." , Value = ""}
            };
            selectPublishers.AddRange(_productService.GetPublishers()
                .Select(p => new SelectListItem
                {
                    Text = p.PublisherName,
                    Value = p.PublisherId.ToString()
                }).ToList());
            ViewBag.Publishers = selectPublishers;

            #endregion

            return View(await _productService.FilterBooksForFilterAdvanced(filter));
        }

        #endregion

        #region CKEditor Images

        [HttpPost("file-upload")]
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
