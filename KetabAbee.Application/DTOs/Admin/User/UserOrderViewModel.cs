using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KetabAbee.Application.DTOs.Admin.User
{
    public class UserOrderViewModel
    {
        public int OrderId { get; set; }

        public string UserName { get; set; }

        public int OrderSum { get; set; }

        public DateTime CreateDate { get; set; }

        public string Address { get; set; }

        public bool IsFinally { get; set; }

        public bool SendingProcessIsCompleted { get; set; }
    }
}
