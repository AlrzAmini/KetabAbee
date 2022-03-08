using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KetabAbee.Domain.Models.Products.Exam
{
    public class CorrectAnswer
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int QuestionId { get; set; }

        [DisplayName("پاسخ صحیح")]
        [MaxLength(200, ErrorMessage = "{0} نمیتواند بیش از {1} کاراکتر داشته باشد")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public string CorrectQuestionAnswer { get; set; }

        #region realtions

        public ExamQuestion Question { get; set; }

        #endregion
    }
}
