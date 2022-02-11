using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KetabAbee.Application.DTOs.Paging;
using KetabAbee.Application.DTOs.Ticket;
using KetabAbee.Application.Interfaces.Ticket;
using KetabAbee.Domain.Interfaces;
using KetabAbee.Domain.Models.Ticket;
using Microsoft.EntityFrameworkCore;

namespace KetabAbee.Application.Services.Ticket
{
    public class TicketService : ITicketService
    {
        private readonly ITicketRepository _ticketRepository;

        public TicketService(ITicketRepository ticketRepository)
        {
            _ticketRepository = ticketRepository;
        }

        public bool AddAnswer(TicketAnswer answer)
        {
            try
            {
                if (answer == null) return false;

                // after add a new answer to ticket
                answer.SendDate = DateTime.Now;
                answer.Ticket.TicketState = TicketState.Answered;
                answer.Ticket.IsReadByAdmin = true;
                answer.Ticket.IsReadBySender = false;
                _ticketRepository.AddAnswer(answer);

                return true;
            }
            catch
            {
                return false;
            }

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

        public bool DeleteTicketById(int ticketId)
        {
            try
            {
                var ticket = _ticketRepository.GetTicketById(ticketId);

                if (ticket == null) return false;

                return _ticketRepository.DeleteTicket(ticket);
            }
            catch
            {
                return false;
            }
        }

        public FilterTicketViewModel FilterTickets(FilterTicketViewModel filter)
        {
            var result = _ticketRepository.GetTicketsForFilter().AsQueryable();

            // filter by create date
            switch (filter.OrderBy)
            {
                case FilterTicketOrder.CreateDateAsc:
                    result = result.OrderBy(t => t.TicketSendDate);
                    break;
                case FilterTicketOrder.CreateDateDsc:
                    result = result.OrderByDescending(t => t.TicketSendDate);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            // filter by priority
            if (filter.TicketPriority != null)
            {
                result = result.Where(r => r.TicketPriority == filter.TicketPriority.Value);
            }

            // filter by state
            if (filter.TicketState != null)
            {
                result = result.Where(r => r.TicketState == filter.TicketState.Value);
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

            //paging
            var pager = Pager.Build(filter.PageNum, result.Count(), filter.Take, filter.PageCountAfterAndBefor);
            var tickets = result.Paging(pager).ToList();

            return filter.SetPaging(pager).SetTickets(tickets);
        }

        public Domain.Models.Ticket.Ticket GetTicketById(int ticketId)
        {
            return _ticketRepository.GetTicketById(ticketId);
        }

        public IEnumerable<Domain.Models.Ticket.Ticket> GetTickets()
        {
            return _ticketRepository.GetTickets();
        }

        public ShowTicketInAdminViewmodel GetTicketForShowTicketInAdmin(int ticketId)
        {
            var ticketWithAnswers = new ShowTicketInAdminViewmodel
            {
                Ticket = _ticketRepository.GetTicketsForTicketDetailsInAdmin()
                    .SingleOrDefault(t => t.TicketId == ticketId),
                Answers = _ticketRepository.GetAnswers()
                    .Where(a => a.TicketId == ticketId)
                    .OrderByDescending(a => a.SendDate)
                    .ToList()
            };

            return ticketWithAnswers;
        }

        public bool TicketIsRead(int ticketId)
        {
            try
            {
                var ticket = GetTicketById(ticketId);

                if (ticket == null)
                {
                    return false;
                }

                if (ticket.IsReadByAdmin)
                {
                    ticket.IsReadByAdmin = false;

                    _ticketRepository.UpdateTicket(ticket);
                    return true;
                }

                ticket.IsReadByAdmin = true;

                _ticketRepository.UpdateTicket(ticket);
                return true;
            }
            catch
            {
                return false;
            }

        }

        public bool CloseTicket(int ticketId)
        {
            try
            {
                var ticket = GetTicketById(ticketId);

                if (ticket == null)
                {
                    return false;
                }

                if (ticket.TicketState == TicketState.Closed)
                {
                    ticket.TicketState = TicketState.Pending;

                    _ticketRepository.UpdateTicket(ticket);
                    return true;
                }

                ticket.TicketState = TicketState.Closed;

                _ticketRepository.UpdateTicket(ticket);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool AddAnswerFromUser(TicketAnswer answer)
        {
            try
            {
                if (answer == null) return false;

                // after add a new answer to ticket
                answer.SendDate = DateTime.Now;
                answer.Ticket.TicketState = TicketState.Pending;
                answer.Ticket.IsReadByAdmin = false;
                answer.Ticket.IsReadBySender = true;
                _ticketRepository.AddAnswer(answer);

                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool TicketIsReadBySender(int ticketId)
        {
            try
            {
                var ticket = GetTicketById(ticketId);

                if (ticket == null)
                {
                    return false;
                }

                ticket.IsReadBySender = true;

                _ticketRepository.UpdateTicket(ticket);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool UserHaveUnReadTicket(int userId)
        {
            return _ticketRepository.UserHaveUnReadTicket(userId);
        }

        public TicketStaticsViewModel GetTicketStaticInfo()
        {
            return new TicketStaticsViewModel
            {
                AllTicketsCount = _ticketRepository.GetTicketsCount(),
                IsReadTicketsCount = _ticketRepository.GetIsReadTicketsCount(),
                ImportantUnReadTicketsCount = _ticketRepository.GetUnReadImportantTicketsCount(),
                UnReadTicketsCount = _ticketRepository.GetUnReadTicketsCount(),
                IsClosedTicketsCount = _ticketRepository.GetClosedTicketsCount()
            };
        }
    }
}
