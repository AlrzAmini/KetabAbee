using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KetabAbee.Domain.Models.Banner;

namespace KetabAbee.Domain.Interfaces
{
    public interface IBannerRepository
    {
        Task<bool> AddBanner(Banner banner);

        Task<bool> UpdateBanner(Banner banner);

        Task<bool> DeleteBanner(Banner banner);

        IEnumerable<Banner> GetBanners();

        Task<Banner> GetBannerById(int bannerId);

        Task<int> GetActiveBannersCountByLocation(BannerLocation bannerLocation);

        Task<List<Banner>> GetAllIsActiveBanners();

        Task<List<Banner>> GetAllIsActiveHeadBanners();

        Task<IQueryable<Banner>> GetAllActiveBannersByLocation(BannerLocation bannerLocation);

    }
}