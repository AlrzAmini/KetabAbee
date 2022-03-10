using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KetabAbee.Domain.Models.Products.Exam;

namespace KetabAbee.Application.DTOs.Admin.Exam
{
    public class ExamDetailsViewModel
    {
        #region properties

        public int ExamId { get; set; }

        public int BookId { get; set; }

        public string BookName { get; set; }

        public int Time { get; set; }

        public bool IsActive { get; set; }

        #endregion


        #region relations

        public ICollection<ExamQuestion> Questions { get; set; }

        #endregion
    }
}
