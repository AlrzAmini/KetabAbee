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
        [Key]
        public int ExamId { get; set; }

        [Required]
        public int BookId { get; set; }

        public int? UserId { get; set; }

        #region properties

        [DisplayName("آی پی")]
        [MaxLength(16)]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public string UserIp { get; set; }

        [DisplayName("تاریخ شروع آزمون")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public DateTime CreateDate { get; set; }

        [DisplayName("تاریخ پایان آزمون")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public DateTime ExpireDate { get; set; }

        public bool IsActive { get; set; }

        public bool IsDelete { get; set; }

        #endregion

        #region relations

        public Book Book { get; set; }

        public User.User User { get; set; }

        public ICollection<ExamQuestion> ExamQuestions { get; set; }

        #endregion
    }
}
