using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KetabAbee.Application.DTOs.Ticket;

namespace KetabAbee.Application.Interfaces.Ticket
{
    public interface ITicketService
    {
        bool AddTicket(AddTicketViewmodel ticket,int senderId);

        IEnumerable<Domain.Models.Ticket.Ticket> GetTickets();

        FilterTicketViewModel FilterTickets(FilterTicketViewModel filter);

        Domain.Models.Ticket.Ticket GetTicketById(int ticketId);

        #region Admin

        bool DeleteTicketById(int ticketId);

        bool TicketIsRead(int ticketId);

        #endregion
    }
}
