using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KetabAbee.Application.DTOs;
using KetabAbee.Application.DTOs.Discount;
using KetabAbee.Application.Interfaces.Product.Discount;

namespace KetabAbee.Web.Areas.AdminPanel.Controllers.Discount
{
    [Route("Admin/Discounts")]
    public class AdminDiscountController : AdminBaseController
    {

        #region constructor

        private readonly IDiscountService _discountService;

        public AdminDiscountController(IDiscountService discountService)
        {
            _discountService = discountService;
        }

        #endregion

        #region index

        [HttpGet]
        public IActionResult Index(FilterDiscountViewModel filter)
        {
            return View(_discountService.FilterDiscount(filter));
        }

        #endregion

        #region Add Discount

        [HttpGet("Create")]
        public IActionResult CreateDiscount()
        {
            return View();
        }

        [HttpPost("Create"), ValidateAntiForgeryToken]
        public IActionResult CreateDiscount(CreateDiscountViewModel discount)
        {
            if (!ModelState.IsValid)
            {
                return View(discount);
            }

            var result = _discountService.Create(discount);

            switch (result)
            {
                case CreateDiscountResult.Success:
                    TempData["SuccessMessage"] = "تخفیف با موفقیت ثبت شد";
                    return RedirectToAction("Index");

                case CreateDiscountResult.NotFound:
                    TempData["ErrorMessage"] = "محصول مورد نظر یافت نشد";
                    return RedirectToAction("CreateDiscount", discount);

                case CreateDiscountResult.Error:
                    TempData["ErrorMessage"] = "خطایی حین انجام عملیات رخ داده است";
                    return RedirectToAction("Index");

                default:
                    return View(discount);
            }
        }

        #endregion

        
    }
}
