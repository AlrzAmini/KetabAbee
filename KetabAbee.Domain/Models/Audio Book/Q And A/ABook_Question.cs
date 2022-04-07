using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KetabAbee.Domain.Models.Audio_Book.Q_And_A
{
    public class ABook_Question
    {
        #region props

        [Key]
        public int Id { get; set; }

        public int? UserId { get; set; }

        public int AudioBookId { get; set; }

        [MaxLength(16)]
        public string UserIp { get; set; }

        [DisplayName("نام")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(400, ErrorMessage = "{0} نمیتواند بیش از {1} کاراکتر داشته باشد")]
        public string UserName { get; set; }

        [DisplayName("عنوان پرسش")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(400, ErrorMessage = "{0} نمیتواند بیش از {1} کاراکتر داشته باشد")]
        public string Title { get; set; }

        [DisplayName("متن پرسش")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(800, ErrorMessage = "{0} نمیتواند بیش از {1} کاراکتر داشته باشد")]
        public string Body { get; set; }

        public bool IsDelete { get; set; }

        public DateTime SendDate { get; set; } = DateTime.Now;

        #endregion

        #region relations

        public User.User User { get; set; }

        public AudioBook AudioBook { get; set; }

        public ICollection<ABook_QAnswer> ABookQAnswers { get; set; }

        #endregion
    }
}
