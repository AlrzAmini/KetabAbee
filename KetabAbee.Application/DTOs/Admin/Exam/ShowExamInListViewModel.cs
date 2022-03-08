using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KetabAbee.Application.DTOs.Admin.Exam
{
    public class ShowExamInListViewModel
    {
        public int ExamId { get; set; }

        public int BookId { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime ExpireDate { get; set; }

        public bool IsActive { get; set; }
    }
}
