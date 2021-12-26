using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KetabAbee.Application.DTOs.Paging;
using KetabAbee.Domain.Models.Ticket;

namespace KetabAbee.Application.DTOs.Ticket
{
    public class AddTicketViewmodel
    {
        #region Properties

        [DisplayName("موضوع تیکت")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(400, ErrorMessage = "{0} نمیتواند بیش از {1} کاراکتر داشته باشد")]
        public string Title { get; set; }

        [DisplayName("متن تیکت")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public string Body { get; set; }

        [DisplayName("اولویت")]
        public TicketPriority TicketPriority { get; set; }

        #endregion
    }

    public class FilterTicketViewModel : BasePaging
    {
        #region Properties

        public string Title { get; set; }

        public int? UserId { get; set; }

        public FilterTicketOrder OrderBy { get; set; }

        public TicketPriority? TicketPriority { get; set; }

        public TicketState? TicketState { get; set; }

        public List<Domain.Models.Ticket.Ticket> Tickets { get; set; }

        #endregion

        #region Methods

        public FilterTicketViewModel SetTickets(List<Domain.Models.Ticket.Ticket> tickets)
        {
            Tickets = tickets;
            return this;
        }

        public FilterTicketViewModel SetPaging(BasePaging paging)
        {
            PageNum = paging.PageNum;
            TotalEntitiesCount = paging.TotalEntitiesCount;
            StartPage = paging.StartPage;
            EndPage = paging.EndPage;
            PageCountAfterAndBefor = paging.PageCountAfterAndBefor;
            Take = paging.Take;
            Skip = paging.Skip;
            TotalPages = paging.TotalPages;

            return this;
        }

        #endregion
    }

    public enum FilterTicketOrder
    {
        [Display(Name = "صعودی")]
        CreateDateAsc,
        [Display(Name = "نزولی")]
        CreateDateDsc

    }
}
