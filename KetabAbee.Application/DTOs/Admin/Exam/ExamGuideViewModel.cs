using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KetabAbee.Application.DTOs.Admin.Exam
{
    public class ExamGuideViewModel
    {
        public string BookName { get; set; }

        public int BookId { get; set; }

        public int ExamId { get; set; }

        public int Time { get; set; }

        public int QuestionsCount { get; set; }
    }
}
