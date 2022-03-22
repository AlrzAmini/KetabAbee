
using KetabAbee.Domain.Models.Banner;

namespace KetabAbee.Application.DTOs.Banner
{
    public class BannerInfoViewModel
    {
        #region properties

        public int BannerId { get; set; }

        public string ImageName { get; set; }

        public string Link { get; set; }

        public string Alt { get; set; }

        public BannerLocation BannerLocation { get; set; }

        public string ImageSavePath { get; set; }

        #endregion
    }
}
