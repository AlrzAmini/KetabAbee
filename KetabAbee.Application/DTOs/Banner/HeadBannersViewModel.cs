using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KetabAbee.Application.DTOs.Banner
{
    public class HeadBannersViewModel
    {
        public BannerInfoViewModel LongHead { get; set; }

        public BannerInfoViewModel FirstShortHead { get; set; }
        public BannerInfoViewModel SecondShortHead { get; set; }

        public List<BannerInfoViewModel> Sliders { get; set; }
    }
}
