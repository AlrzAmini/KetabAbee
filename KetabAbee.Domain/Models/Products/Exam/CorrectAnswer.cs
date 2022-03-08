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
        #region properties

        [Key]
        public int Id { get; set; }

        [Required]
        public int QuestionId { get; set; }

        [DisplayName("جواب صحیح")]
        public int QuestionAnswerId { get; set; }

        public bool IsDelete { get; set; }

        #endregion

        #region relations

        public ExamQuestion Question { get; set; }

        public QuestionAnswer QuestionAnswer { get; set; }

        #endregion
    }
}
