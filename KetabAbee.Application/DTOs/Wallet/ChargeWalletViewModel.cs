using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KetabAbee.Application.DTOs.Wallet
{
    public class ChargeWalletViewModel
    {
        [DisplayName("مبلغ")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public long Amount { get; set; }

        [DisplayName("بابت")]
        [MaxLength(400,ErrorMessage = "{0} نمیتواند بیش از {1} کاراکتر داشته باشد")]
        public string Behalf { get; set; }
    }
}
