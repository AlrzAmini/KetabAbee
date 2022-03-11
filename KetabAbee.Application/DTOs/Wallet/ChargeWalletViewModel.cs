using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KetabAbee.Application.DTOs.Site;

namespace KetabAbee.Application.DTOs.Wallet
{
    public class ChargeWalletViewModel : CaptchaViewModel
    {
        [DisplayName("مبلغ")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [Range(1000, 10000000, ErrorMessage = "{0} باید بین {1} و {2} باشد")]
        public long Amount { get; set; }

        [DisplayName("بابت")]
        [MaxLength(400, ErrorMessage = "{0} نمیتواند بیش از {1} کاراکتر داشته باشد")]
        public string Behalf { get; set; }

        public bool IsPay { get; set; }
    }
}
