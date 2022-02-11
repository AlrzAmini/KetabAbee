using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KetabAbee.Application.DTOs.Ticket
{
    public class TicketStaticsViewModel
    {
        public int AllTicketsCount { get; set; }

        public int IsReadTicketsCount { get; set; }

        public int UnReadTicketsCount { get; set; }

        public int ImportantUnReadTicketsCount { get; set; }

        public int IsClosedTicketsCount { get; set; }
    }
}
