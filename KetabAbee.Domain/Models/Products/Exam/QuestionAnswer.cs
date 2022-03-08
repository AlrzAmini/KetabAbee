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
        [Key]
        public int AnswerId { get; set; }

        [Required]
        public int QuestionId { get; set; }

        #region properties

        [DisplayName("متن پاسخ")]
        [MaxLength(200, ErrorMessage = "{0} نمیتواند بیش از {1} کاراکتر داشته باشد")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public string AnswerBody { get; set; }

        public bool IsDelete { get; set; }

        #endregion

        #region relations

        public ExamQuestion Question { get; set; }

        #endregion
    }
}
