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

            return View(_walletService.GetWalletsWithPagingByUserId(walletsWithPaging));
        }

        [HttpPost("Wallet/Charge")]
        public IActionResult ChargeWallet(ChargeWalletViewModel charge)
        {
            if (!ModelState.IsValid)
            {
                var walletWithPaging = new WalletsWithPagingViewModel
                {
                    UserId = User.GetUserId()
                };
                var newWallets = _walletService.GetWalletsWithPagingByUserId(walletWithPaging);
                return View(newWallets);
            }

            // check price cant be 0
            if (charge.Amount <= 0)
            {
                TempData["ErrorMessage"] = "مبلغ وارد شده صحیح نمی باشد";
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
                TempData["SuccessMessage"] = "شارژ حساب شما با موفقیت انجام شد . لذت ببرید";
                return Redirect("/UserPanel/Dashboard");
            }

            TempData["ErrorMessage"] = "عملیات شارژ حساب انجام نشد";
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
