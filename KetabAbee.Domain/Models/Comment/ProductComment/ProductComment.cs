using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KetabAbee.Domain.Models.Products;

namespace KetabAbee.Domain.Models.Comment.ProductComment
{
    public class ProductComment
    {
        [Key]
        public int CommentId { get; set; }

        public int? UserId { get; set; }

        [Required]
        public int ProductId { get; set; }

        #region props

        [Required]
        [MaxLength(100)]
        public string UserIp { get; set; }

        [DisplayName("نام")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(150, ErrorMessage = "{0} نمیتواند بیش از {1} کاراکتر داشته باشد")]
        public string UserName { get; set; }

        [DisplayName("ایمیل")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(256, ErrorMessage = "{0} نمیتواند بیش از {1} کاراکتر داشته باشد")]
        [EmailAddress(ErrorMessage = "ایمیل وارد شده معتبر نیست . دقت کنید !")]
        public string Email { get; set; }

        [DisplayName("تاریخ ارسال")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public DateTime SendDate { get; set; }

        public bool IsReadByAdmin { get; set; }

        public bool IsDelete { get; set; }

        #endregion


        #region relations

        public User.User User { get; set; }

        public Book Product { get; set; }

        #endregion
    }
}
