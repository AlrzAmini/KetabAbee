using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KetabAbee.Domain.Models.Products.Exam
{
    public class ExamResult
    {
        #region properties

        [Key]
        public int ExamResultId { get; set; }

        [Required]
        public int ExamId { get; set; }

        public int? UserId { get; set; }

        [MaxLength(16)]
        public string UserIp { get; set; }

        [DisplayName("نمره")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [Range(0, 100, ErrorMessage = "{0} باید بین {1} و {2} باشد")]
        public int Score { get; set; }

        public int AllQuestionsCount { get; set; }
        public int CorrectAnswersCount { get; set; }
        public int WrongAnswersCount { get; set; }

        public ExamResultStatus ResultStatus { get; set; }

        #endregion

        #region relations

        public Exam Exam { get; set; }

        public User.User User { get; set; }

        #endregion
    }

    public enum ExamResultStatus
    {
        Pass,
        Failed
    }
}
