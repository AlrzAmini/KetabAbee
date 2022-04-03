
using KetabAbee.Domain.Models.Audio_Book;
using KetabAbee.Domain.Models.Banner;

namespace KetabAbee.Application.Extensions
{
    public static class PathExtensions
    {
        public static string DefaultImage = "wwwroot/images/Defualt.jpg";

        #region Banner

        public static string BannerImageSavePath = "/images/banner";
        public static string GetBannerAddress(this Banner banner)
        {
            return BannerImageSavePath + "/" + banner.ImageName;
        }
        public static string BannerFullAddress(string imageName)
        {
            return BannerImageSavePath + "/" + imageName;
        }

        #endregion

        #region audio book

        public static string AudioBookImageSavePath = "/images/audiobook/img";
        public static string AudioBookThumbSavePath = "/images/audiobook/thumb";
        public static string AudioBookFileSavePath = "/files/audiobook";

        public static string AudioBookImageFullAddress(string imageName)
        {
            return AudioBookImageSavePath + "/" + imageName;
        }

        public static string GetAudioBookAddress(this AudioBook book)
        {
            return AudioBookImageSavePath + "/" + book.ImageName;
        }

        public static string AudioBookThumbFullAddress(string imageName)
        {
            return AudioBookThumbSavePath + "/" + imageName;
        }

        public static string AudioBookFileFullAddress(string fileName)
        {
            return AudioBookFileSavePath + "/" + fileName;
        }

        #endregion
    }
}
