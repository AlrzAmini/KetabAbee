using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KetabAbee.Application.DTOs.Wallet;
using KetabAbee.Application.Extensions;
using KetabAbee.Application.Interfaces.Wallet;

namespace KetabAbee.Web.Areas.UserPanel.Controllers
{
    public class WalletController : UserPanelBaseController
    {
        #region cunstructor

        private readonly IWalletService _walletService;

        public WalletController(IWalletService walletService)
        {
            _walletService = walletService;
        }

        #endregion

        #region Charge Wallet

        [HttpGet("Wallet/Charge")]
        public IActionResult ChargeWallet(WalletsWithPagingViewModel walletsWithPaging)
        {
            walletsWithPaging.UserId = User.GetUserId();
            ViewBag.WalletList = _walletService.GetWalletsWithPagingByUserId(walletsWithPaging);
            return View();
        }

        [HttpPost("Wallet/Charge")]
        public IActionResult ChargeWallet(ChargeWalletViewModel charge)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.WalletList = _walletService.GetWalletsByUserId(User.GetUserId());
                return View(charge);
            }


            var walletId = _walletService.ChargeWalletByUserId(User.GetUserId(), charge);
           
            #region Payment

            var payment = new ZarinpalSandbox.Payment((int)charge.Amount);
            var res = payment.PaymentRequest(charge.Behalf, "https://localhost:44338/Wallet/OnlinePayment/" + walletId,"mranotmillion@gmail.com","09300804882");
            if (res.Result.Status == 100)
            {
               return Redirect("https://sandbox.zarinpal.com/pg/startpay/"+res.Result.Authority);
            }

            #endregion
           
            return Redirect("/Wallet/Charge");
        }

        #endregion

    }
}
