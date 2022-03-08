using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KetabAbee.Domain.Models.Products.Exam
{
    public class ExamQuestion
    {
        #region peroperties

        [Key]
        public int QuestionId { get; set; }

        [Required]
        public int ExamId { get; set; }

        [DisplayName("متن سوال")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(500, ErrorMessage = "{0} نمیتواند بیش از {1} کاراکتر داشته باشد")]
        public string QBody { get; set; }

        public bool IsDelete { get; set; }

        #endregion


        #region realtions

        public Exam Exam { get; set; }

        public ICollection<QuestionAnswer> QuestionAnswers { get; set; }

        #endregion
    }
}
