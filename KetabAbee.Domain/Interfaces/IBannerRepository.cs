using System.Collections.Generic;
using System.Threading.Tasks;
using KetabAbee.Domain.Models.Banner;

namespace KetabAbee.Domain.Interfaces
{
    public interface IBannerRepository
    {
        Task<bool> AddBanner(Banner banner);

        Task<bool> UpdateBanner(Banner banner);

        Task<bool> DeleteBanner(Banner banner);

        Task<List<Banner>> GetBanners();
    }
}