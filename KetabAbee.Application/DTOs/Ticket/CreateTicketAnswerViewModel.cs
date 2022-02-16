using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KetabAbee.Application.DTOs.Site;

namespace KetabAbee.Application.DTOs.Ticket
{
    public class CreateTicketAnswerViewModel : CaptchaViewModel
    {
        public int TicketId { get; set; }

        public int SenderId { get; set; }

        [DisplayName("متن پاسخ")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public string AnswerBody { get; set; }

        public Domain.Models.Ticket.Ticket Ticket { get; set; }
    }
}
