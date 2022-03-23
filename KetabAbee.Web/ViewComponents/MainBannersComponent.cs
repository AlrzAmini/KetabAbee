using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KetabAbee.Application.Interfaces.Banner;
using Microsoft.AspNetCore.Mvc;

namespace KetabAbee.Web.ViewComponents
{
    public class MainBannersComponent : ViewComponent
    {
        private readonly IBannerService _bannerService;

        public MainBannersComponent(IBannerService bannerService)
        {
            _bannerService = bannerService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View("MainBannersComponent", await _bannerService.GetMainBannersForShow());
        }
    }
}
