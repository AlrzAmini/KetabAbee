using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KetabAbee.Domain.Models.Banner;
using Microsoft.AspNetCore.Http;

namespace KetabAbee.Application.DTOs.Admin.Banner
{
    public class CreateBannerViewModel
    {
        #region properties

        [DisplayName("لینک بنر")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(500)]
        public string Link { get; set; }

        [DisplayName("محل بنر در سایت")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public BannerLocation BannerLocation { get; set; }

        [DisplayName("تاریخ شروع")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public string StartDate { get; set; }

        [DisplayName("تاریخ پایان")]
        public string EndDate { get; set; }

        [DisplayName("وضعیت")]
        public bool IsActive { get; set; }

        public IFormFile Image { get; set; }

        #endregion
    }
}
