using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KetabAbee.Application.DTOs.Site
{
    public class CaptchaViewModel
    {
        [DisplayName("Captcha")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public string Captcha { get; set; }
    }
}
