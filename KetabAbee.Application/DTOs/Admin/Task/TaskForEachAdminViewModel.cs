using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KetabAbee.Domain.Models.Task;

namespace KetabAbee.Application.DTOs.Admin.Task
{
    public class TaskForEachAdminViewModel
    {
        public int TaskId { get; set; }

        public string CreatorName { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime DeadLine { get; set; }

        public bool IsCompleted { get; set; }

        public TaskPriority TaskPriority { get; set; }

        public string Body { get; set; }
    }
}
