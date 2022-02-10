using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace KetabAbee.Application.DTOs.Admin.Blog
{
    public class EditBlogViewModel
    {
        #region props

        [DisplayName("عنوان")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(300, ErrorMessage = "{0} نمیتواند بیش از {1} کاراکتر داشته باشد")]
        public string BlogTitle { get; set; }

        [DisplayName("متن")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public string BlogBody { get; set; }

        [DisplayName("توضیحات صفحه")]
        [MinLength(200, ErrorMessage = "{0} نمیتواند کمتر از {1} کاراکتر داشته باشد")]
        [MaxLength(300, ErrorMessage = "{0} نمیتواند بیش از {1} کاراکتر داشته باشد")]
        public string PageDescription { get; set; }

        [DisplayName("کلمات کلیدی")]
        [MaxLength(200, ErrorMessage = "{0} نمیتواند بیش از {1} کاراکتر داشته باشد")]
        public string Tags { get; set; }

        [DisplayName("نویسنده")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(300, ErrorMessage = "{0} نمیتواند بیش از {1} کاراکتر داشته باشد")]
        public string Writer { get; set; }

        public IFormFile Image { get; set; }

        public int UserId { get; set; }

        public string ImageName { get; set; }

        public int BlogId { get; set; }

        #endregion
    }

    public enum BlogEditResult
    {
        Success,
        NotFound,
        Failed,
        UnHandledError
    }
}
