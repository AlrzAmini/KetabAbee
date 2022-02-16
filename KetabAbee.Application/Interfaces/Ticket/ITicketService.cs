using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KetabAbee.Application.DTOs.Ticket;
using KetabAbee.Domain.Models.Ticket;

namespace KetabAbee.Application.Interfaces.Ticket
{
    public interface ITicketService
    {

        #region Ticket

        bool AddTicket(AddTicketViewmodel ticket, int senderId);

        IEnumerable<Domain.Models.Ticket.Ticket> GetTickets();

        FilterTicketViewModel FilterTickets(FilterTicketViewModel filter);

        Domain.Models.Ticket.Ticket GetTicketById(int ticketId);

        bool TicketIsReadBySender(int ticketId);

        bool UserHaveUnReadTicket(int userId);

        #region Admin

        bool DeleteTicketById(int ticketId);

        bool TicketIsRead(int ticketId);

        bool CloseTicket(int ticketId);

        TicketStaticsViewModel GetTicketStaticInfo();

        #endregion

        #endregion

        #region Answer

        bool AddAnswer(TicketAnswer answer);

        ShowTicketInAdminViewmodel GetTicketForShowTicketInAdmin(int ticketId);

        bool AddAnswerFromUser(CreateTicketAnswerViewModel answer);

        #endregion

    }
}
