using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KetabAbee.Domain.Models.Task;

namespace KetabAbee.Application.DTOs.Admin.Task
{
    public class TaskInfoViewModel
    {
        public int TaskId { get; set; }

        public string CreatorName { get; set; }

        public string RoleTitle { get; set; }

        #region properties

        public string Body { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime? DeadLine { get; set; }

        public TaskPriority TaskPriority { get; set; }

        public bool IsCompleted { get; set; }

        #endregion
    }
}
