using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KetabAbee.Domain.Models.Products.Exam
{
    public class ExamGrade
    {
        [Key]
        public int GradeId { get; set; }

        [Required]
        public int ExamId { get; set; }

        [DisplayName("نمره")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [Range(0, 10)]
        public int Grade { get; set; }

        #region relations

        public Exam Exam { get; set; }

        #endregion
    }
}
