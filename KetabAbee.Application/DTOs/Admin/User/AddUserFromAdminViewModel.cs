using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace KetabAbee.Application.DTOs.Admin.User
{
    public class AddUserFromAdminViewModel
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
        [MaxLength(400, ErrorMessage = "{0} نمیتواند بیش از {1} کاراکتر داشته باشد")]
        [EmailAddress(ErrorMessage = "ایمیل وارد شده معتبر نیست . دقت کنید !")]
        public string Email { get; set; }

        #endregion

        #region Password

        [DisplayName("کلمه عبور")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(200, ErrorMessage = "{0} نمیتواند بیش از {1} کاراکتر داشته باشد")]
        public string Password { get; set; }

        #endregion

        #region Mobile

        [DisplayName("موبایل")]
        [MaxLength(50, ErrorMessage = "{0} نمیتواند بیش از {1} کاراکتر داشته باشد")]
        public string Mobile { get; set; }

        #endregion

        #region Birth Day

        [DisplayName("تاریخ تولد")]
        public DateTime? BirthDay { get; set; }

        #endregion

        #region Avatar

        public IFormFile Avatar { get; set; }

        #endregion

        #region Roles

        public List<int> SelectedRoles { get; set; }

        #endregion
    }
}
