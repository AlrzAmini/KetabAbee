using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using GoogleReCaptcha.V3.Interface;
using KetabAbee.Application.DTOs.Wallet;
using KetabAbee.Application.Extensions;
using KetabAbee.Application.Interfaces.Wallet;
using Microsoft.AspNetCore.Authorization;

namespace KetabAbee.Web.Areas.UserPanel.Controllers
{
    public class WalletController : UserPanelBaseController
    {
        #region cunstructor

        private readonly IWalletService _walletService;
        private readonly ICaptchaValidator _captchaValidator;
        private readonly IPaymentService _paymentService;

        public WalletController(IWalletService walletService, ICaptchaValidator captchaValidator, IPaymentService paymentService)
        {
            _walletService = walletService;
            _captchaValidator = captchaValidator;
            _paymentService = paymentService;
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

        [HttpPost("Wallet/Charge"), ValidateAntiForgeryToken]
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
            var callbackUrl = CommonExtensions.DomainAddress + Url.RouteUrl("ZarinPalPaymentResultWallet", new { amount = charge.Amount, behalf = charge.Behalf});
            var redirectUrl = "";
            var status = _paymentService.CreatePaymentRequest(null, int.Parse(charge.Amount.ToString()), charge.Behalf, callbackUrl, ref redirectUrl, User.GetUserEmail());

            if (status == PaymentStatus.St100)
            {
                return Redirect(redirectUrl);
            }

            return Redirect(!string.IsNullOrEmpty(url) ? url : "/UserPanel/Dashboard");

        }

        #endregion

        #region zarin pal call back

        [AllowAnonymous]
        [HttpGet("zarin-wallet-pay-result", Name = "ZarinPalPaymentResultWallet")]
        public IActionResult WalletCallBackZarinPal(int amount, string behalf)
        {
            var authority = _paymentService.GetAuthorityCodeFromCallback(HttpContext);
            if (authority == "")
            {
                TempData["ErrorSwal"] = "پرداخت با شکست مواجه شد";
                return View();
            }
            long refId = 0;
            var res = _paymentService.PaymentVerification(null, authority, amount, ref refId);

            if (res == PaymentStatus.St100)
            {
                var charge = new ChargeWalletViewModel
                {
                    Amount = amount,
                    Behalf = behalf,
                    IsPay = true
                };
                if (_walletService.ChargeWalletByUserId(User.GetUserId(), charge))
                {
                    TempData["SuccessSwal"] = "پرداخت موفق";
                    ViewBag.refId = refId;
                    return View();
                }
                TempData["ErrorSwal"] = "پرداخت ناموفق";
                return View();
            }

            TempData["ErrorSwal"] = "پرداخت ناموفق";
            return View();
        }

        #endregion
    }
}
