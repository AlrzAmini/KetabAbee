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
    public class Ticket
    {
        #region Properties

        [Key]
        public int TicketId { get; set; }

        [Required]
        public int SenderId { get; set; }

        [DisplayName("موضوع تیکت")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(400, ErrorMessage = "{0} نمیتواند بیش از {1} کاراکتر داشته باشد")]
        public string Title { get; set; }

        [DisplayName("متن تیکت")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public string Body { get; set; }

        [Required]
        public DateTime TicketSendDate { get; set; }

        public bool IsReadBySender { get; set; }
        public bool IsReadByAdmin { get; set; }

        [DisplayName("اولویت")]
        public TicketPriority TicketPriority { get; set; }

        [DisplayName("وضعیت")]
        public TicketState TicketState { get; set; }

        #endregion

        #region Relations

        public User.User Sender { get; set; }

        public ICollection<TicketAnswer> Answers { get; set; }

        #endregion
    }

    public enum TicketPriority
    {
        [Display(Name = "کم")]
        Low,
        [Display(Name = "معمولی")]
        Medium,
        [Display(Name = "زیاد")]
        High
    }

    public enum TicketState
    {
        [Display(Name = "در حال بررسی")]
        Pending,
        [Display(Name = "پاسخ داده شده")]
        Answered,
        [Display(Name = "بسته شده")]
        Closed
    }
}
