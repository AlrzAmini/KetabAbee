using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KetabAbee.Application.Extensions;
using Microsoft.AspNetCore.Http;

namespace KetabAbee.Application.DTOs.Admin.AudioBook
{
    public class CreateAndEditAudioBookViewModel
    {
        #region props

        public int AudioBookId { get; set; }

        [DisplayName("عنوان")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(350, ErrorMessage = "{0} نمیتواند بیش از {1} کاراکتر داشته باشد")]
        public string Title { get; set; }

        [DisplayName("نویسنده")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(350, ErrorMessage = "{0} نمیتواند بیش از {1} کاراکتر داشته باشد")]
        public string Writer { get; set; }

        [DisplayName("گوینده")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(350, ErrorMessage = "{0} نمیتواند بیش از {1} کاراکتر داشته باشد")]
        public string Speaker { get; set; }

        [DisplayName("نقد و بررسی")]
        public string Review { get; set; }

        [DisplayName("توضیحات صفحه")]
        [MinLength(200, ErrorMessage = "{0} نمیتواند کمتر از {1} کاراکتر داشته باشد")]
        [MaxLength(300, ErrorMessage = "{0} نمیتواند بیش از {1} کاراکتر داشته باشد")]
        public string PageDescription { get; set; }

        [MaxLength(50, ErrorMessage = "{0} نمیتواند بیش از {1} کاراکتر داشته باشد")]
        public string ImageName { get; set; }

        [MaxLength(50, ErrorMessage = "{0} نمیتواند بیش از {1} کاراکتر داشته باشد")]
        public string FileName { get; set; }

        [DisplayName("حجم فایل")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public float FileSize { get; set; }

        public IFormFile Image { get; set; }

        public IFormFile File { get; set; }

        #endregion
    }

    public enum CreateAudioBookResult
    {
        Success,
        Error,
        ValidationFileError
    }
    public enum EditAudioBookResult
    {
        Success,
        Error,
        ValidationFileError,
        NotFound
    }
}
