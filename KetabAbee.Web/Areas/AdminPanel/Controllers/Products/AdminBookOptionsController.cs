using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KetabAbee.Application.Const;
using KetabAbee.Application.Interfaces.Product;
using KetabAbee.Application.Security;

namespace KetabAbee.Web.Areas.AdminPanel.Controllers.Products
{
    [PermissionChecker(PerIds.Admin‌Books)]
    [Route("Admin/Book/Option")]
    public class AdminBookOptionsController : AdminBaseController
    {
        #region constructor

        private readonly IProductService _productService;

        public AdminBookOptionsController(IProductService productService)
        {
            _productService = productService;
        }

        #endregion

        #region buyers

        [HttpGet("{bookId}/Buyers")]
        public IActionResult BookBuyers(int bookId)
        {
            var users = _productService.GetBookUsers(bookId);
            if (users.Any()) return View(users);

            TempData["ErrorMessage"] = "این کتاب خریداری ندارد";
            return RedirectToAction("BookInfo", "AdminBook", new { bookId });
        }

        #endregion

        #region Lottery

        [HttpGet("{bookId}/Lottery")]
        public IActionResult Lottery(int bookId)
        {
            var winnerId = _productService.GetLotteryWinner(bookId);

            var sa = winnerId;
            return View();
        }

        #endregion
    }
}
