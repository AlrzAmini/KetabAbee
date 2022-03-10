using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KetabAbee.Application.DTOs.Admin.Exam
{
    public class EditExamViewModel
    {
        [DisplayName("محصول")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public int productId { get; set; }

        public int ExamId { get; set; }

        public string ProductName { get; set; }

        [DisplayName("زمان آزمون")]
        [Range(1, 30, ErrorMessage = "{0} باید بین {1} و {2} باشد")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public int Time { get; set; }
    }
}
