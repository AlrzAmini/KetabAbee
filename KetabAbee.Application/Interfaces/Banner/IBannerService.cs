using System.Threading.Tasks;
using KetabAbee.Application.DTOs.Admin;
using KetabAbee.Application.DTOs.Admin.Banner;
using KetabAbee.Application.DTOs.Banner;
using KetabAbee.Domain.Models.Banner;

namespace KetabAbee.Application.Interfaces.Banner
{
    public interface IBannerService
    {
        PagingBannersViewModel GetBannersWithPaging(PagingBannersViewModel model);

        Task<CreateBannerResult> CreateBanner(CreateBannerViewModel banner);

        Task<DeleteBannerResult> DeleteBannerById(int bannerId);

        Task<bool> CheckBannerLimitations(BannerLocation bannerLocation, int? increaseValue);

        Task<HeadBannersViewModel> GetHeadBanners();

        Task<ActiveBannerResult> ActiveBanner(int bannerId);
        Task<bool> DeActiveBanner(int bannerId);
    }
}