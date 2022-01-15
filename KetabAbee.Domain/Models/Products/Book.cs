using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KetabAbee.Domain.Models.Products
{
    public class Book
    {
        [Key]
        public int BookId { get; set; }

        public int PublisherId { get; set; }

        public int GroupId { get; set; }

        public AgeRange AgeRange { get; set; }

        public CoverType CoverType { get; set; }

        public ExistStatus ExistStatus { get; set; }


        #region Props

        [DisplayName("نام کتاب")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(300, ErrorMessage = "{0} نمیتواند بیش از {1} کاراکتر داشته باشد")]
        public string Name { get; set; }

        [DisplayName("قیمت")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
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

        public bool IsDelete { get; set; }


        #endregion

        #region Relations

        public Publisher Publisher { get; set; }

        public ProductGroup Group { get; set; }


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

    public enum ExistStatus
    {
        [Display(Name = "موجود")]
        Exist,
        [Display(Name = "موجود در انبار")]
        InStock,
        [Display(Name = "نا موجود")]
        IsNotExist
    }

}
