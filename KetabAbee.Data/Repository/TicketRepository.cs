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

        public int GetClosedTicketsCount()
        {
            return _context.Tickets.Count(t => t.TicketState == TicketState.Closed);
        }

        public int GetIsReadTicketsCount()
        {
            return _context.Tickets.Count(t => t.IsReadByAdmin);
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

        public int GetTicketsCount()
        {
            return _context.Tickets.Count();
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

        public int GetUnReadImportantTicketsCount()
        {
            return _context.Tickets.Count(t => !t.IsReadByAdmin && t.TicketPriority == TicketPriority.High);
        }

        public int GetUnReadTicketsCount()
        {
            return _context.Tickets.Count(t => !t.IsReadByAdmin);
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
