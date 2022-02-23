using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KetabAbee.Application.DTOs.Site;

namespace KetabAbee.Application.DTOs
{
    public class RegisterViewModel : CaptchaViewModel
    {
        #region UserName

        [DisplayName("نام کاربری")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(200, ErrorMessage = "{0} نمیتواند بیش از {1} کاراکتر داشته باشد")]
        public string UserName { get; set; }

        #endregion

        #region Email

        [DisplayName("ایمیل")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(200, ErrorMessage = "{0} نمیتواند بیش از {1} کاراکتر داشته باشد")]
        [EmailAddress(ErrorMessage = "بخدا که هم تو هم من میدونیم این ایمیلت نیست . اذیت نکن دوست من")]
        public string Email { get; set; }

        #endregion

        #region Password

        [DisplayName("کلمه عبور")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(200, ErrorMessage = "{0} نمیتواند بیش از {1} کاراکتر داشته باشد")]
        [MinLength(6, ErrorMessage = "همه جا همینجوری رمز وارد می کنی ستون ؟ {0} باید حداقل {1} کاراکتر داشته باشه")]
        public string Password { get; set; }

        #endregion

        #region Confirm Password

        [DisplayName("تکرار کلمه عبور")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(200, ErrorMessage = "{0} نمیتواند بیش از {1} کاراکتر داشته باشد")]
        [Compare("Password", ErrorMessage = "رمز عبور و تکرار آن با هم یکسان نیستند")]
        public string ConfirmPassword { get; set; }

        #endregion

        #region UserIp

        [MaxLength(15)]
        public string UserIp { get; set; }

        #endregion

    }

    public class LoginViewModel : CaptchaViewModel
    {
        #region Email

        [DisplayName("ایمیل")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(200, ErrorMessage = "{0} نمیتواند بیش از {1} کاراکتر داشته باشد")]
        [EmailAddress(ErrorMessage = "فرمت ایمیل وارد شده نادرست است")]
        public string Email { get; set; }

        #endregion

        #region Password

        [DisplayName("کلمه عبور")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(200, ErrorMessage = "{0} نمیتواند بیش از {1} کاراکتر داشته باشد")]
        public string Password { get; set; }

        #endregion

        public bool RememberMe { get; set; }

    }

    public class ForgotPasswordViewModel : CaptchaViewModel
    {
        #region Email

        [DisplayName("ایمیل")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(200, ErrorMessage = "{0} نمیتواند بیش از {1} کاراکتر داشته باشد")]
        [EmailAddress(ErrorMessage = "فرمت ایمیل وارد شده نادرست است")]
        public string Email { get; set; }

        #endregion
    }

    public class ResetPasswordViewModel : CaptchaViewModel
    {
        #region Password

        [DisplayName("کلمه عبور")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(200, ErrorMessage = "{0} نمیتواند بیش از {1} کاراکتر داشته باشد")]
        public string Password { get; set; }

        #endregion

        #region Confirm Password

        [DisplayName("تکرار کلمه عبور")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(200, ErrorMessage = "{0} نمیتواند بیش از {1} کاراکتر داشته باشد")]
        [Compare("Password", ErrorMessage = "رمز عبور و تکرار آن با هم یکسان نیستند")]
        public string ConfirmPassword { get; set; }

        #endregion

        public string EmailActiveCode { get; set; }
    }
}
