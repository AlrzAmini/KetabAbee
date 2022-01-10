using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KetabAbee.Application.DTOs.Admin.Wallet
{
    public class ChargeWalletFromAdminViewModel
    {
        public int UserId { get; set; }

        public string UserName { get; set; }

        [DisplayName("مبلغ")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public long Amount { get; set; }

        [DisplayName("بابت")]
        [MaxLength(400, ErrorMessage = "{0} نمیتواند بیش از {1} کاراکتر داشته باشد")]
        public string Behalf { get; set; }

        public string Inventory { get; set; }

    }
}
