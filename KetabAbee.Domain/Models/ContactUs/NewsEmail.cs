using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KetabAbee.Domain.Models.ContactUs
{
    public class NewsEmail
    {
        [Key]
        public int Id { get; set; }

        #region props

        [DisplayName("ایمیل")]
        [MaxLength(256, ErrorMessage = "{0} نمیتواند بیش از {1} کاراکتر داشته باشد")]
        [EmailAddress(ErrorMessage = "ایمیل وارد شده معتبر نیست . دقت کنید !")]
        public string EmailAddress { get; set; }

        public bool IsDelete { get; set; }

        #endregion

        #region relations

        public ICollection<NewsLetter> NewsLetters { get; set; }

        #endregion
    }
}
