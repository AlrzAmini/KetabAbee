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
        public IActionResult ChargeWallet()
        {
            ViewBag.WalletList = _walletService.GetWalletsByUserId(User.GetUserId());
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

            // charge
            if (_walletService.ChargeWalletByUserId(User.GetUserId(), charge))
            {
                TempData["SuccessMessage"] = "عملیات با موفقیت انجام شد";
                return Redirect("/Wallet/Charge");
            }
            TempData["ErrorMessage"] = "عملیات با شکست مواجه شد";
            return Redirect("/Wallet/Charge");

            // online payment

        }

        #endregion

    }
}
