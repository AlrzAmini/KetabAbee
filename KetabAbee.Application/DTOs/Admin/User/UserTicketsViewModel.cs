using System;
using System.ComponentModel;
using KetabAbee.Domain.Models.Ticket;

namespace KetabAbee.Application.DTOs.Admin.User
{
    public class UserTicketsViewModel
    {
        #region Properties

        public int TicketId { get; set; }

        public string SenderName { get; set; }

        public string Title { get; set; }

        public string Body { get; set; }

        public DateTime TicketSendDate { get; set; }

        public bool IsReadBySender { get; set; }

        public bool IsReadByAdmin { get; set; }

        [DisplayName("اولویت")]
        public TicketPriority TicketPriority { get; set; }

        [DisplayName("وضعیت")]
        public TicketState TicketState { get; set; }

        #endregion
    }
}
