using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace KetabAbee.Application.DTOs
{
    public class UserInformationInDashboardViewmodel
    {
        public string UserName { get; set; }

        public string Email { get; set; }

        public string Mobile { get; set; }

        public int Age { get; set; }

        public long Wallet { get; set; }

        public DateTime RegisterDate { get; set; }
    }

    public class UserPanelSideBarViewmodel
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public long Wallet { get; set; }
        public string AvatarName { get; set; }
    }

    public class UserPanelEditInfoViewModel
    {

        #region Mobile

        [DisplayName("موبایل")]
        [MaxLength(50, ErrorMessage = "{0} نمیتواند بیش از {1} کاراکتر داشته باشد")]
        public string Mobile { get; set; }

        #endregion

        public int Age { get; set; }

        public string Email { get; set; }

        #region Avatar

        public IFormFile UserAvatar { get; set; }

        public string CurrentAvatar { get; set; }

        #endregion


    }

    public class ChangePasswordViewModel
    {
        #region old Password

        [DisplayName("کلمه عبور فعلی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(200, ErrorMessage = "{0} نمیتواند بیش از {1} کاراکتر داشته باشد")]
        public string OldPassword { get; set; }

        #endregion

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
    }
}
