using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        #endregion

    }
}
