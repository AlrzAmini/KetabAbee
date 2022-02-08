using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KetabAbee.Application.DTOs;
using KetabAbee.Application.Interfaces.Product.Discount;

namespace KetabAbee.Web.Areas.AdminPanel.Controllers.Discount
{
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

        [HttpGet("Admin/Discounts")]
        public IActionResult Index(FilterDiscountViewModel filter)
        {
            return View(_discountService.FilterDiscount(filter));
        }

        #endregion
    }
}
