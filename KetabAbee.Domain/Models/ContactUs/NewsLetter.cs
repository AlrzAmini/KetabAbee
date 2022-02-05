
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace KetabAbee.Domain.Models.ContactUs
{
    public class NewsLetter
    {
        [Key]
        public int Id { get; set; }

        #region props

        [DisplayName("عنوان")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(150, ErrorMessage = "{0} نمیتواند بیش از {1} کاراکتر داشته باشد")]
        public string Subject { get; set; }

        [DisplayName("متن")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(600, ErrorMessage = "{0} نمیتواند بیش از {1} کاراکتر داشته باشد")]
        public string Body { get; set; }

        [Required]
        public DateTime SendDate { get; set; }

        public bool IsSend { get; set; }

        #endregion

        #region relations

        public ICollection<NewsEmail> NewsEmails { get; set; }

        #endregion
    }
}
