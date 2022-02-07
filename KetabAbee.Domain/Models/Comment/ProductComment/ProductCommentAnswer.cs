using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace KetabAbee.Domain.Models.Comment.ProductComment
{
    public class ProductCommentAnswer
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int CommentId { get; set; }

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

        [DisplayName("متن پاسخ")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public string AnswerBody { get; set; }

        [DisplayName("تاریخ ارسال")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public DateTime SendDate { get; set; }

        public bool IsDelete { get; set; }

        #endregion

        #region relations

        public User.User User { get; set; }

        public ProductComment Comment { get; set; }

        #endregion
    }
}
