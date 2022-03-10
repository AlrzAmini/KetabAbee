using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KetabAbee.Domain.Models.Products.Exam
{
    public class QuestionAnswer
    {
        #region properties

        [Key]
        public int QAnswerId { get; set; }

        [Required]
        public int QuestionId { get; set; }

        [DisplayName("متن گزینه")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(300, ErrorMessage = "{0} نمیتواند بیش از {1} کاراکتر داشته باشد")]
        public string QAnswerBody { get; set; }

        public bool IsCorrect { get; set; }

        public bool IsDelete { get; set; }


        #endregion

        #region relations

        public ExamQuestion Question { get; set; }

        #endregion
    }
}
