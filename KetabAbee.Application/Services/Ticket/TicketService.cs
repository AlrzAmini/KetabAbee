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

        public FilterTicketViewModel FilterTickets(FilterTicketViewModel filter)
        {
            var result = _ticketRepository.GetTickets().AsQueryable();

            // filter by create date
            switch (filter.OrderBy)
            {
                case FilterTicketOrder.CreateDateAsc:
                    result = result.OrderBy(t => t.TicketSendDate);
                    break;
                case FilterTicketOrder.CreateDateDsc:
                    result = result.OrderByDescending(t => t.TicketSendDate);
                    break;
            }

            // filter by priority
            if (filter.TicketPriority != null)
            {
                result = result.Where(r => r.TicketPriority == filter.TicketPriority.Value);
            }

            //filter by user Id
            if (filter.UserId != null)
            {
                result = result.Where(r => r.SenderId == filter.UserId);
            }

            //filter by title
            if (!string.IsNullOrEmpty(filter.Title))
            {
                result = result.Where(r => r.Title.Contains(filter.Title));
            }

            filter.Tickets = result.ToList();

            return filter;
        }

        public Domain.Models.Ticket.Ticket GetTicketById(int ticketId)
        {
            return _ticketRepository.GetTicketById(ticketId);
        }

        public IEnumerable<Domain.Models.Ticket.Ticket> GetTickets()
        {
            return _ticketRepository.GetTickets();
        }
    }
}
