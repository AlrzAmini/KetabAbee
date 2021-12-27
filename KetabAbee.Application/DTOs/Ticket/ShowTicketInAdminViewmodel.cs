using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KetabAbee.Domain.Models.Ticket;

namespace KetabAbee.Application.DTOs.Ticket
{
    public class ShowTicketInAdminViewmodel
    {
        public Domain.Models.Ticket.Ticket Ticket { get; set; }

        public List<TicketAnswer> Answers { get; set; }
    }
}
