using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using GoogleReCaptcha.V3.Interface;
using KetabAbee.Application.DTOs.Wallet;
using KetabAbee.Application.Extensions;
using KetabAbee.Application.Interfaces.Wallet;

namespace KetabAbee.Web.Areas.UserPanel.Controllers
{
    public class WalletController : UserPanelBaseController
    {
        #region cunstructor

        private readonly IWalletService _walletService;
        private readonly ICaptchaValidator _captchaValidator;

        public WalletController(IWalletService walletService, ICaptchaValidator captchaValidator)
        {
            _walletService = walletService;
            _captchaValidator = captchaValidator;
        }

        #endregion

        #region Charge Wallet

        [HttpGet("Wallet/Charge")]
        public IActionResult ChargeWallet(WalletsWithPagingViewModel walletsWithPaging, string url, int amount, string behalf)
        {
            walletsWithPaging.UserId = User.GetUserId();

            if (string.IsNullOrEmpty(url)) return View(_walletService.GetWalletsWithPagingByUserId(walletsWithPaging));
            ViewBag.AmountFromCart = amount;
            ViewBag.BehalfFromCart = behalf;
            ViewBag.UrlFromCart = url;

            return View(_walletService.GetWalletsWithPagingByUserId(walletsWithPaging));
        }

        [HttpPost("Wallet/Charge"),ValidateAntiForgeryToken]
        public async Task<IActionResult> ChargeWallet(ChargeWalletViewModel charge, string url)
        {
            if (!await _captchaValidator.IsCaptchaPassedAsync(charge.Captcha))
            {
                TempData["ErrorSwal"] = "احراز هویت کپچا انجام نشد چند لحظه دیگر تلاش کنید";
                return Redirect("/Wallet/Charge");
            }

            if (!ModelState.IsValid)
            {
                var walletWithPaging = new WalletsWithPagingViewModel
                {
                    UserId = User.GetUserId()
                };
                var newWallets = _walletService.GetWalletsWithPagingByUserId(walletWithPaging);
                return View(newWallets);
            }

            // charge wallet
            if (_walletService.ChargeWalletByUserId(User.GetUserId(), charge))
            {
                TempData["SuccessSwal"] = "شارژ حساب شما با موفقیت انجام شد . لذت ببرید";
                return Redirect(!string.IsNullOrEmpty(url) ? url : "/UserPanel/Dashboard");
            }

            TempData["ErrorSwal"] = "عملیات شارژ حساب انجام نشد";
            return Redirect("/Wallet/Charge");

            #region Payment

            //var payment = new ZarinpalSandbox.Payment((int)charge.Amount);
            //var res = payment.PaymentRequest(charge.Behalf, "https://localhost:44338/Wallet/OnlinePayment/" + walletId, "mranotmillion@gmail.com", "09300804882");
            //if (res.Result.Status == 100)
            //{
            //    return Redirect("https://sandbox.zarinpal.com/pg/startpay/" + res.Result.Authority);
            //}

            #endregion
        }

        #endregion
    }
}
