using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KetabAbee.Application.Interfaces.Banner;
using Microsoft.AspNetCore.Mvc;

namespace KetabAbee.Web.ViewComponents
{
    public class MainAndProfileBannersComponent : ViewComponent
    {
        private readonly IBannerService _bannerService;

        public MainAndProfileBannersComponent(IBannerService bannerService)
        {
            _bannerService = bannerService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var banners = await _bannerService.GetMainAndProfileBannersForShow();
            return View("MainAndProfileBanners", banners);
        }
    }
}
