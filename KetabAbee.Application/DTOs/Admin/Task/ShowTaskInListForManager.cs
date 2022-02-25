using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KetabAbee.Application.DTOs.Admin.Task
{
    public class ShowTaskInListForManager
    {
        public int TaskId { get; set; }

        public string CreatorName { get; set; }

        public string RoleTitle { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime? DeadLine { get; set; }
    }
}
