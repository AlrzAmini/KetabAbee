
using KetabAbee.Domain.Models.Banner;

namespace KetabAbee.Application.Extensions
{
    public static class PathExtensions
    {
        public static string DefaultImage = "wwwroot/images/Defualt.jpg";

        public static string BannerImageSavePath = "/images/banner";
        public static string GetBannerAddress(this Banner banner)
        {
            return BannerImageSavePath + "/" + banner.ImageName;
        }
        public static string BannerFullAddress(string imageName)
        {
            return BannerImageSavePath + "/" + imageName;
        }
    }
}
