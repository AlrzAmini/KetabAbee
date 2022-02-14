using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KetabAbee.Domain.Models.ContactUs
{
    public class RequestBranch
    {
        #region props

        [Key]
        public int ReqId { get; set; }

        [DisplayName("نام و نام خانوادگی")]
        [MaxLength(200, ErrorMessage = "{0} نمیتواند بیش از {1} کاراکتر داشته باشد")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public string Name { get; set; }

        [DisplayName("تلفن")]
        [MaxLength(50, ErrorMessage = "{0} نمیتواند بیش از {1} کاراکتر داشته باشد")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public string Phone { get; set; }

        [DisplayName("ایمیل")]
        [MaxLength(256, ErrorMessage = "{0} نمیتواند بیش از {1} کاراکتر داشته باشد")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public string Address { get; set; }

        [Required]
        public DateTime CreateDate { get; set; }

        #endregion
    }
}
