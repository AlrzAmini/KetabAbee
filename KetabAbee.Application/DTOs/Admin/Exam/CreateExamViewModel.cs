using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KetabAbee.Application.DTOs.Admin.Exam
{
    public class CreateExamViewModel
    {
        [DisplayName("محصول")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public int productId { get; set; }

        [DisplayName("متن سوال")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(500, ErrorMessage = "{0} نمیتواند بیش از {1} کاراکتر داشته باشد")]
        public string Question { get; set; }

        [DisplayName("متن گزینه اول")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(300, ErrorMessage = "{0} نمیتواند بیش از {1} کاراکتر داشته باشد")]
        public string QAnswer1 { get; set; }

        [DisplayName("متن گزینه دوم")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(300, ErrorMessage = "{0} نمیتواند بیش از {1} کاراکتر داشته باشد")]
        public string QAnswer2 { get; set; }

        [DisplayName("متن گزینه سوم")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(300, ErrorMessage = "{0} نمیتواند بیش از {1} کاراکتر داشته باشد")]
        public string QAnswer3 { get; set; }

        [DisplayName("متن گزینه چهارم")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(300, ErrorMessage = "{0} نمیتواند بیش از {1} کاراکتر داشته باشد")]
        public string QAnswer4 { get; set; }

        [DisplayName("زمان آزمون")]
        [Range(1, 30, ErrorMessage = "{0} باید بین {1} و {2} باشد")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public int Time { get; set; }
    }
}
