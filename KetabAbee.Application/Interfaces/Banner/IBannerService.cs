using System.Threading.Tasks;
using KetabAbee.Application.DTOs.Admin;
using KetabAbee.Application.DTOs.Admin.Banner;
using KetabAbee.Domain.Models.Banner;

namespace KetabAbee.Application.Interfaces.Banner
{
    public interface IBannerService
    {
        PagingBannersViewModel GetBannersWithPaging(PagingBannersViewModel model);

        Task<bool> CreateBanner(CreateBannerViewModel banner);

        Task<DeleteBannerResult> DeleteBannerById(int bannerId);
    }
}