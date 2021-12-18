﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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


}
