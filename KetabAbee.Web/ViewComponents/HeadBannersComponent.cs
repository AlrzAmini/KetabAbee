using System.Threading.Tasks;
using KetabAbee.Application.Const;
using KetabAbee.Application.Extensions;
using KetabAbee.Application.Interfaces.Banner;
using Microsoft.AspNetCore.Mvc;

namespace KetabAbee.Web.ViewComponents
{
    public class HeadBannersComponent : ViewComponent
    {
        private readonly IBannerService _bannerService;

        public HeadBannersComponent(IBannerService bannerService)
        {
            _bannerService = bannerService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var banners = await _bannerService.GetHeadBanners();
            return View("HeadBanners", banners);
        }
    }
}