using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KetabAbee.Domain.Models.Audio_Book
{
    public class AudioBook
    {
        #region properties

        [Key]
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
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
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
        [Range(0, 1000, ErrorMessage = "{0} باید بین {1} و {2} باشد")]
        public float FileSize { get; set; }

        [DisplayName("زمان")]
        public int Time { get; set; }

        public DateTime CreateDate { get; set; } = DateTime.Now;

        public bool IsDelete { get; set; }

        #endregion
    }
}
