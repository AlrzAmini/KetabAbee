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

        public Ticket GetTicketById(int ticketId)
        {
            return _context.Tickets.Include(t=>t.Sender).SingleOrDefault(t=>t.TicketId == ticketId);
        }

        public IEnumerable<Ticket> GetTickets()
        {
            return _context.Tickets;
        }

        public void UpdateTicket(Ticket ticket)
        {
            _context.Tickets.Update(ticket);
            _context.SaveChanges();
        }
    }
}
