using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KetabAbee.Application.DTOs.Ticket;
using KetabAbee.Application.Interfaces.Ticket;
using KetabAbee.Domain.Interfaces;
using KetabAbee.Domain.Models.Ticket;

namespace KetabAbee.Application.Services.Ticket
{
    public class TicketService : ITicketService
    {
        private readonly ITicketRepository _ticketRepository;

        public TicketService(ITicketRepository ticketRepository)
        {
            _ticketRepository = ticketRepository;
        }

        public bool AddTicket(AddTicketViewmodel ticket, int senderId)
        {
            if (string.IsNullOrEmpty(ticket.Body)) return false;

            var newTicket = new Domain.Models.Ticket.Ticket()
            {
                SenderId = senderId,
                Title = ticket.Title,
                Body = ticket.Body,
                TicketPriority = ticket.TicketPriority,
                TicketSendDate = DateTime.Now,
                IsReadBySender = true,
                TicketState = TicketState.Pending
            };
            
            return _ticketRepository.AddTicket(newTicket);
        }

        public IEnumerable<Domain.Models.Ticket.Ticket> GetTickets()
        {
            return _ticketRepository.GetTickets();
        }
    }
}
