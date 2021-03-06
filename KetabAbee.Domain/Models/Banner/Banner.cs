using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KetabAbee.Domain.Models.Banner
{
    public class Banner
    {
        #region properties

        [Key]
        public int BannerId { get; set; }

        [DisplayName("نام تصویر")]
        [MaxLength(50)]
        public string ImageName { get; set; }

        [DisplayName("لینک بنر")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(500)]
        public string Link { get; set; }

        [DisplayName("توضیح بنر")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(250)]
        public string Alt { get; set; }

        [DisplayName("محل بنر در سایت")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public BannerLocation BannerLocation { get; set; }

        [DisplayName("تاریخ شروع")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public DateTime StartDate { get; set; }

        [DisplayName("تاریخ پایان")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public DateTime EndDate { get; set; }

        public bool IsDelete { get; set; }

        [DisplayName("وضعیت")]
        public bool IsActive { get; set; }

        #endregion
    }

    public enum BannerLocation
    {
        [Display(Name = "بلند بالای صفحه")]
        LongHead,
        [Display(Name = "کوتاه بالای صفحه")]
        ShortHead,
        [Display(Name = "وسط صفحه و پروفایل")]
        MainAndProfile,
        [Display(Name = "وسط صفحه")]
        Main,
        [Display(Name = "اسلایدر")]
        Slider
    }

    public enum DeleteBannerResult
    {
        Success,
        Error,
        ImageError,
        NotFounded
    }

    public enum CreateBannerResult
    {
        Success,
        Error,
        OutOfRangeBanner
    }

    public enum ActiveBannerResult
    {
        Success,
        Error,
        OutOfRange,
        NotFounded
    }

    public enum EditBannerResult
    {
        Success,
        Error,
        OutOfRange,
        NotFounded,
        ImageError
    }
}
