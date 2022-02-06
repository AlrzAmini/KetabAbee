using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KetabAbee.Domain.Models.Order;
using KetabAbee.Domain.Models.User;

namespace KetabAbee.Domain.Models.Products
{
    public class Book
    {
        [Key]
        public int BookId { get; set; }

        [DisplayName("ناشر")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public int PublisherId { get; set; }

        [DisplayName("دسته بندی اصلی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public int GroupId { get; set; }

        [DisplayName("دسته بندی فرعی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public int SubGroupId { get; set; }

        [DisplayName("دسته بندی فرعی دوم")]
        public int? SubGroup2Id { get; set; }

        [DisplayName("رنج سنی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public AgeRange AgeRange { get; set; }

        [DisplayName("نوع جلد")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public CoverType CoverType { get; set; }


        #region Props

        [DisplayName("نام کتاب")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(300, ErrorMessage = "{0} نمیتواند بیش از {1} کاراکتر داشته باشد")]
        public string Name { get; set; }

        [DisplayName("قیمت")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [Range(0,999999, ErrorMessage = "{0} باید بین {1} و {2} باشد")]
        public int Price { get; set; }

        [DisplayName("نویسنده")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(300, ErrorMessage = "{0} نمیتواند بیش از {1} کاراکتر داشته باشد")]
        public string Writer { get; set; }

        [DisplayName("تعداد صفحه")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public int PagesCount { get; set; }

        [MaxLength(50, ErrorMessage = "{0} نمیتواند بیش از {1} کاراکتر داشته باشد")]
        public string ImageName { get; set; }

        [DisplayName("خلاصه کتاب")]
        public string Abstract { get; set; }

        [DisplayName("موجودی")]
        public int? Inventory { get; set; }

        public bool IsDelete { get; set; }


        #endregion

        #region Relations

        public Publisher Publisher { get; set; }

        public ProductGroup Group { get; set; }

        public ProductGroup SubGroup { get; set; }

        public ProductGroup SubGroup2 { get; set; }

        public ICollection<FavoriteBook> FavoriteBook { get; set; }

        public ICollection<OrderDetail> OrderDetails { get; set; }

        public ICollection<InventoryReport> InventoryReports { get; set; }

        public ICollection<UserBook> UserBooks { get; set; }

        public ICollection<BookScore> BookScores { get; set; }


        #endregion
    }

    public enum AgeRange
    {
        [Display(Name = "کودک")]
        Kid,
        [Display(Name = "نوجوان")]
        Teenager,
        [Display(Name = "بزرگسال")]
        Adult
    }

    public enum CoverType
    {
        [Display(Name = "شومیز")]
        PaperBacked,
        [Display(Name = "کاغذی")]
        Paper,
        [Display(Name = "سخت")]
        Hardcover
    }

}
