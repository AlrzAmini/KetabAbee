using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KetabAbee.Application.Const;
using KetabAbee.Application.DTOs.Admin;
using KetabAbee.Application.DTOs.Admin.Banner;
using KetabAbee.Application.Interfaces.Banner;
using KetabAbee.Application.Security;
using KetabAbee.Domain.Models.Banner;

namespace KetabAbee.Web.Areas.AdminPanel.Controllers.Banner
{
    [PermissionChecker(PerIds.AdminBanners)]
    [Route("Admin/Banners")]
    public class AdminBannerController : AdminBaseController
    {
        #region costructor

        private readonly IBannerService _bannerService;

        public AdminBannerController(IBannerService bannerService)
        {
            _bannerService = bannerService;
        }

        #endregion

        #region index

        [HttpGet]
        public IActionResult Index(PagingBannersViewModel model)
        {
            return View(_bannerService.GetBannersWithPaging(model));
        }

        #endregion

        #region add banner

        [HttpGet("Add")]
        public IActionResult AddBanner()
        {
            return View();
        }

        [HttpPost("Add"), ValidateAntiForgeryToken]
        public async Task<IActionResult> AddBanner(CreateBannerViewModel banner)
        {
            if (!ModelState.IsValid)
            {
                return View(banner);
            }

            var res = await _bannerService.CreateBanner(banner);

            switch (res)
            {
                case CreateBannerResult.Success:
                    TempData["SuccessMessage"] = "بنر با موفقیت افزوده شد";
                    return RedirectToAction("Index");
                case CreateBannerResult.Error:
                    TempData["ErrorMessage"] = "بنر افزوده نشد";
                    return RedirectToAction("Index");
                case CreateBannerResult.OutOfRangeBanner:
                    TempData["WarningMessage"] = "تعداد بنر های فعال در ناحیه مکانی انتخابی شما نمیتواند بیش از مقدار کنونی باشد";
                    return RedirectToAction("AddBanner");
                default:
                    TempData["ErrorMessage"] = "بنر افزوده نشد";
                    return RedirectToAction("Index");
            }
        }

        #endregion

        #region delete banner

        [HttpGet("Delete/{bannerId}")]
        public async Task<IActionResult> DeleteBanner(int bannerId)
        {
            var res = await _bannerService.DeleteBannerById(bannerId);
            switch (res)
            {
                case DeleteBannerResult.Success:
                    TempData["SuccessMessage"] = "با موفقیت حذف شد";
                    return RedirectToAction("Index");
                case DeleteBannerResult.NotFounded:
                    TempData["WarningMessage"] = "بنر مورد نظر یافت نشد";
                    return RedirectToAction("Index");
                case DeleteBannerResult.ImageError:
                    TempData["WarningMessage"] = "مشکلی در ثبت تصویر رخ داد";
                    return RedirectToAction("Index");
                case DeleteBannerResult.Error:
                    TempData["ErrorMessage"] = "مشکلی در ثبت بنر رخ داد";
                    return RedirectToAction("Index");
                default:
                    return RedirectToAction("Index");
            }
        }

        #endregion

        #region active banner

        [HttpGet("Active/{bannerId}")]
        public async Task<IActionResult> ActiveBanner(int bannerId)
        {
            var res = await _bannerService.ActiveBanner(bannerId);
            switch (res)
            {
                case ActiveBannerResult.Success:
                    TempData["SuccessMessage"] = "فعال شد";
                    return RedirectToAction("Index");
                case ActiveBannerResult.Error:
                    TempData["ErrorMessage"] = "فعال نشد";
                    return RedirectToAction("Index");
                case ActiveBannerResult.NotFounded:
                    TempData["ErrorMessage"] = "یافت نشد";
                    return RedirectToAction("Index");
                case ActiveBannerResult.OutOfRange:
                    TempData["WarningMessage"] = "تعداد بنر های فعال در ناحیه مکانی انتخابی شما نمیتواند بیش از مقدار کنونی باشد";
                    return RedirectToAction("Index");
                default:
                    return RedirectToAction("Index");
            }
        }

        #endregion

        #region de active banner

        [HttpGet("DeActive/{bannerId}")]
        public async Task<IActionResult> DeActiveBanner(int bannerId)
        {
            if (await _bannerService.DeActiveBanner(bannerId))
            {
                TempData["SuccessMessage"] = "غیرفعال شد";
                return RedirectToAction("Index");
            }
            TempData["ErrorMessage"] = "غیرفعال نشد";
            return RedirectToAction("Index");
        }

        #endregion

        #region edit banner

        [HttpGet("Edit/{bannerId}")]
        public IActionResult EditBanner(int bannerId)
        {
            return View();
        }

        #endregion
    }
}
