using KetabAbee.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using KetabAbee.Application.DTOs;
using KetabAbee.Application.DTOs.Book;
using KetabAbee.Application.Interfaces.Product;
using KetabAbee.Application.Interfaces.User;
using KetabAbee.Application.Interfaces.Wallet;
using Microsoft.AspNetCore.Http;

namespace KetabAbee.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUserService _userService;
        private readonly IWalletService _walletService;
        private readonly IProductService _productService;

        public HomeController(IUserService userService, IWalletService walletService, IProductService productService)
        {
            _userService = userService;
            _walletService = walletService;
            _productService = productService;
        }

        [HttpGet]
        public IActionResult Index(FilterBookListViewModel filter)
        {
            return View(_productService.GetBooksForIndex(filter));
        }

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
