using KetabAbee.Application.Const;
using Microsoft.AspNetCore.Mvc;
using KetabAbee.Application.DTOs;
using KetabAbee.Application.DTOs.Admin.Discount;
using KetabAbee.Application.DTOs.Discount;
using KetabAbee.Application.Interfaces.Product.Discount;
using KetabAbee.Application.Security;

namespace KetabAbee.Web.Areas.AdminPanel.Controllers.Discount
{
    [PermissionChecker(PerIds.AdminDiscounts)]
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

        #region Create Discount

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

        #region remove discount

        [HttpGet("Remove/{discountId}")]
        public IActionResult RemoveDiscount(int discountId)
        {
            if (_discountService.RemoveDiscount(discountId))
            {
                TempData["SuccessMessage"] = "تخفیف با موفقیت حذف شد";
                return RedirectToAction("Index");
            }
            TempData["ErrorMessage"] = "تخفیف حذف نشد";
            return RedirectToAction("Index");
        }

        #endregion

        #region edit discount

        [HttpGet("Edit/{discountId}")]
        public IActionResult EditDiscount(int discountId)
        {
            var discount = _discountService.GetInfoForEditDiscount(discountId);

            if (discount == null)
            {
                return NotFound();
            }

            return View(discount);
        }

        [HttpPost("Edit/{discountId}"),ValidateAntiForgeryToken]
        public IActionResult EditDiscount(EditDiscountViewModel discount)
        {
            if (!ModelState.IsValid)
            {
                return View(discount);
            }

            var result = _discountService.EditDiscount(discount);

            switch (result)
            {
                case EditDiscountResult.Success:
                    TempData["SuccessMessage"] = "تخفیف با موفقیت ویرایش شد";
                    return RedirectToAction("Index");

                case EditDiscountResult.NotFound:
                    TempData["ErrorMessage"] = "تخفیف مورد نظر یافت نشد";
                    return RedirectToAction("CreateDiscount");

                case EditDiscountResult.Error:
                    TempData["ErrorMessage"] = "خطایی حین انجام عملیات رخ داده است";
                    return RedirectToAction("Index");

                case EditDiscountResult.UnHandledException:
                    TempData["ErrorMessage"] = "خطایی پیش بینی نشده ای رخ داد";
                    return RedirectToAction("Index");

                default:
                    return View(discount);
            }
        }

        #endregion
    }
}
