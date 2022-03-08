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
        [Key]
        public int QuestionId { get; set; }

        [Required]
        public int ExamId { get; set; }

        #region properties

        [DisplayName("متن سوال")]
        [MaxLength(300,ErrorMessage = "{0} نمیتواند بیش از {1} کاراکتر داشته باشد")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public string QuestionBody { get; set; }

        public bool IsDelete { get; set; }

        #endregion

        #region relations

        public Exam Exam { get; set; }

        public ICollection<QuestionAnswer> QuestionAnswers { get; set; }

        #endregion
    }
}
