using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KetabAbee.Domain.Models.Ticket
{
    public class TicketAnswer
    {
        [Key]
        public int AnswerId { get; set; }

        [Required]
        public int TicketId { get; set; }

        public int SenderId { get; set; }


        #region Props

        [DisplayName("متن پاسخ")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public string AnswerBody { get; set; }

        [Required]
        public DateTime SendDate { get; set; }

        public bool IsDelete { get; set; }

        #endregion

        #region Relations

        public Ticket Ticket { get; set; }

        public User.User Sender { get; set; }

        #endregion

    }
}
