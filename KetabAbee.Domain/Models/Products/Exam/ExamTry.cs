using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KetabAbee.Domain.Models.Products.Exam
{
    public class ExamTry
    {
        #region props

        [Key]
        public int TryId { get; set; }

        public int? UserId { get; set; }

        public int ExamId { get; set; }

        [MaxLength(16)]
        public string UserIp { get; set; }

        public DateTime TryDate { get; set; } = DateTime.Now;

        #endregion

        #region relations

        public User.User User { get; set; }

        public Exam Exam { get; set; }

        #endregion
    }
}
