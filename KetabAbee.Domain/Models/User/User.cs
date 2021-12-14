using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        [EmailAddress(ErrorMessage = "ایمیل دارد شده معتبر نیست . دقت کنید !")]
        public string Email { get; set; }

        #endregion

        #region Password

        [DisplayName("کلمه عبور")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(200, ErrorMessage = "{0} نمیتواند بیش از {1} کاراکتر داشته باشد")]
        public string Password { get; set; }

        #endregion

        #region Active Code

        [DisplayName("کد فعالسازی")]
        [MaxLength(50, ErrorMessage = "{0} نمیتواند بیش از {1} کاراکتر داشته باشد")]
        public string ActivationCode { get; set; }

        #endregion

        #region Age

        [DisplayName("سن")]
        public int Age { get; set; }

        #endregion

        #region IsActive

        [DisplayName("وضعیت")]
        public bool IsActive { get; set; }

        #endregion

        #region User Avatar

        [DisplayName("تصویر")]
        [MaxLength(50, ErrorMessage = "{0} نمیتواند بیش از {1} کاراکتر داشته باشد")]
        public string Avatar { get; set; }

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

        #endregion
    }
}
