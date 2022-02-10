using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KetabAbee.Domain.Models.Blog
{
    public class Blog
    {

        [Key]
        public int BlogId { get; set; }

        [Required]
        public int UserId { get; set; }


        #region props

        [DisplayName("عنوان")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(300, ErrorMessage = "{0} نمیتواند بیش از {1} کاراکتر داشته باشد")]
        public string BlogTitle { get; set; }

        [DisplayName("متن")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public string BlogBody { get; set; }

        [DisplayName("توضیحات صفحه")]
        [MinLength(200, ErrorMessage = "{0} نمیتواند بیش از {1} کاراکتر داشته باشد")]
        [MaxLength(300, ErrorMessage = "{0} نمیتواند بیش از {1} کاراکتر داشته باشد")]
        public string PageDescription { get; set; }

        [DisplayName("کلمات کلیدی")]
        [MaxLength(200, ErrorMessage = "{0} نمیتواند بیش از {1} کاراکتر داشته باشد")]
        public string Tags { get; set; }

        [MaxLength(50)]
        public string ImageName { get; set; }

        [Required]
        public DateTime CreateDate { get; set; }

        public bool IsDelete { get; set; }

        #endregion

        #region relations

        public User.User User { get; set; }

        #endregion
    }
}
