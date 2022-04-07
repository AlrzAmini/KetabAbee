using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KetabAbee.Domain.Models.Audio_Book
{
    public class AudioBookRequest
    {
        #region props

        [Key]
        public string Id { get; set; }

        public int AudioBookId { get; set; }

        [MaxLength(16)]
        public string UserIp { get; set; }

        [DisplayName("نام کاربر")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(300, ErrorMessage = "{0} نمیتواند بیش از {1} کاراکتر داشته باشد")]
        public string UserName { get; set; }

        [DisplayName("عنوان کتاب")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(300, ErrorMessage = "{0} نمیتواند بیش از {1} کاراکتر داشته باشد")]
        public string BookTitle { get; set; }

        #endregion

        #region relations

        public AudioBook AudioBook { get; set; }

        #endregion
    }
}
