using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KetabAbee.Domain.Models.Comment.ProductComment;
using KetabAbee.Domain.Models.Products;
using KetabAbee.Domain.Models.Ticket;

namespace KetabAbee.Domain.Models.User
{
    public class User
    {
        #region UserId

        [Key]
        public int UserId { get; set; }

        #endregion

        #region UserName

        [DisplayName("نام کاربری")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(200, ErrorMessage = "{0} نمیتواند بیش از {1} کاراکتر داشته باشد")]
        public string UserName { get; set; }

        #endregion

        #region Email

        [DisplayName("ایمیل")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(400, ErrorMessage = "{0} نمیتواند بیش از {1} کاراکتر داشته باشد")]
        [EmailAddress(ErrorMessage = "ایمیل وارد شده معتبر نیست . دقت کنید !")]
        public string Email { get; set; }

        #endregion

        #region Mobile

        [DisplayName("موبایل")]
        [MaxLength(50, ErrorMessage = "{0} نمیتواند بیش از {1} کاراکتر داشته باشد")]
        public string Mobile { get; set; }

        #endregion

        #region Password

        [DisplayName("کلمه عبور")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(200, ErrorMessage = "{0} نمیتواند بیش از {1} کاراکتر داشته باشد")]
        public string Password { get; set; }

        #endregion

        #region Email Active Code

        [DisplayName("کد فعالسازی ایمیل")]
        [MaxLength(50, ErrorMessage = "{0} نمیتواند بیش از {1} کاراکتر داشته باشد")]
        public string EmailActivationCode { get; set; }

        #endregion

        #region Email Active Code

        [DisplayName("کد فعالسازی موبایل")]
        [MaxLength(20, ErrorMessage = "{0} نمیتواند بیش از {1} کاراکتر داشته باشد")]
        public string MobileActivationCode { get; set; }

        #endregion

        #region Age

        [DisplayName("سن")]
        public int? Age { get; set; }

        #endregion

        #region Birth Day

        [DisplayName("تاریخ تولد")]
        public DateTime? BirthDay { get; set; }

        #endregion

        #region Address

        [DisplayName("آدرس")]
        [MaxLength(700)]
        public string Address { get; set; }

        #endregion

        #region Is email Active

        public bool IsEmailActive { get; set; }

        #endregion
         
        #region Is mobile Active

        [DisplayName("وضعیت")]
        public bool IsMobileActive { get; set; }

        #endregion

        #region User Avatar

        [DisplayName("تصویر")]
        [MaxLength(50, ErrorMessage = "{0} نمیتواند بیش از {1} کاراکتر داشته باشد")]
        public string AvatarName { get; set; }

        #endregion

        #region Register Date

        [DisplayName("تاریخ ثبت نام")]
        public DateTime RegisterDate { get; set; }

        #endregion

        #region Is delete ?

        public bool IsDelete { get; set; }

        #endregion

        #region Relations

        public List<UserRole> UserRoles { get; set; }

        public List<Ticket.Ticket> Tickets { get; set; }

        public ICollection<TicketAnswer> TicketAnswers { get; set; }

        public ICollection<Wallet.Wallet> Wallets { get; set; }

        public ICollection<FavoriteBook> FavoriteBooks { get; set; }

        public ICollection<Order.Order> Orders { get; set; }

        public ICollection<UserBook> UserBooks { get; set; }

        public ICollection<ContactUs.ContactUs> ContactUsCollection { get; set; }

        public ICollection<BookScore> BookScores { get; set; }

        public ICollection<ProductComment> ProductComments { get; set; }

        #endregion
    }
}
