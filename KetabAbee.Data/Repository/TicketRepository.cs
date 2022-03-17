using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KetabAbee.Data.Context;
using KetabAbee.Domain.Interfaces;
using KetabAbee.Domain.Models.Ticket;
using Microsoft.EntityFrameworkCore;

namespace KetabAbee.Data.Repository
{
    public class TicketRepository : ITicketRepository
    {
        private readonly KetabAbeeDBContext _context;

        public TicketRepository(KetabAbeeDBContext context)
        {
            _context = context;
        }

        public bool AddAnswer(TicketAnswer answer)
        {
            _context.TicketAnswers.Add(answer);
            _context.SaveChanges();

            return true;
        }

        public bool AddTicket(Ticket ticket)
        {
            try
            {
                _context.Tickets.Add(ticket);
                _context.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool DeleteTicket(Ticket ticket)
        {
            _context.Tickets.Remove(ticket);
            _context.SaveChanges();

            return true;
        }

        public IEnumerable<TicketAnswer> GetAnswers()
        {
            return _context.TicketAnswers
                .Include(a => a.Sender);
        }

        public async Task<int> GetClosedTicketsCount()
        {
            return await _context.Tickets.CountAsync(t => t.TicketState == TicketState.Closed);
        }

        public async Task<int> GetIsReadTicketsCount()
        {
            return await _context.Tickets.CountAsync(t => t.IsReadByAdmin);
        }

        public Ticket GetTicketById(int ticketId)
        {
            return _context.Tickets
                .Include(t => t.Sender)
                .SingleOrDefault(t => t.TicketId == ticketId);
        }

        public IEnumerable<Ticket> GetTickets()
        {
            return _context.Tickets;
        }

        public async Task<int> GetTicketsCount()
        {
            return await _context.Tickets.CountAsync();
        }

        public IEnumerable<Ticket> GetTicketsForFilter()
        {
            return _context.Tickets
                .Include(t => t.Sender)
                .OrderByDescending(t=>t.TicketSendDate);
        }

        public IEnumerable<Ticket> GetTicketsForTicketDetailsInAdmin()
        {
            return _context.Tickets
                  .Include(t => t.Sender)
                  .Include(t => t.Answers);
        }

        public async Task<int> GetUnReadImportantTicketsCount()
        {
            return await _context.Tickets.CountAsync(t => !t.IsReadByAdmin && t.TicketPriority == TicketPriority.High);
        }

        public async Task<int> GetUnReadTicketsCount()
        {
            return await _context.Tickets.CountAsync(t => !t.IsReadByAdmin);
        }

        public void UpdateTicket(Ticket ticket)
        {
            _context.Tickets.Update(ticket);
            _context.SaveChanges();
        }

        public bool UserHaveUnReadTicket(int userId)
        {
            return _context.Tickets.Any(t => t.SenderId == userId && !t.IsReadBySender);
        }
    }
}
