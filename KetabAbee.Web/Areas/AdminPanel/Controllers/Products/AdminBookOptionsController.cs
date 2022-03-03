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
        private readonly IProductService _productService;

        public AdminBookOptionsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet("Buyers")]
        public IActionResult BookBuyers(int bookId)
        {
            var users = _productService.GetBookUsers(bookId);
            if (users.Any()) return View(users);

            TempData["ErrorMessage"] = "این کتاب خریداری ندارد";
            return RedirectToAction("Index", "AdminBook");
        }
    }
}
