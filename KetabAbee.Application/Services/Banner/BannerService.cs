using KetabAbee.Application.Interfaces.Banner;
using KetabAbee.Domain.Interfaces;

namespace KetabAbee.Application.Services.Banner
{
    public class BannerService : IBannerService
    {
        private readonly IBannerRepository _bannerRepository;

        public BannerService(IBannerRepository bannerRepository)
        {
            _bannerRepository = bannerRepository;
        }
    }
}