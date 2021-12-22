﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KetabAbee.Domain.Models.Ticket;

namespace KetabAbee.Domain.Interfaces
{
    public interface ITicketRepository
    {
        bool AddTicket(Ticket ticket);

        Ticket GetTicketById(int ticketId);

        IEnumerable<Ticket> GetTickets();
    }
}