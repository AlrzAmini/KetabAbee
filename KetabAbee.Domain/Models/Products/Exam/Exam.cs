using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KetabAbee.Domain.Models.Products.Exam
{
    public class Exam
    {
        #region properties

        [Key]
        public int ExamId { get; set; }

        [Required]
        public int BookId { get; set; }

        [DisplayName("زمان آزمون")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [Range(0, 30, ErrorMessage = "{0} باید بین {1} و {2} باشد")]
        public int Time { get; set; }

        public bool IsActive { get; set; }

        public bool IsDelete { get; set; }

        #endregion


        #region relations

        public Book Book { get; set; }

        public ICollection<ExamQuestion> Questions { get; set; }

        public ICollection<ExamTry> ExamTries { get; set; }

        #endregion
    }
}
