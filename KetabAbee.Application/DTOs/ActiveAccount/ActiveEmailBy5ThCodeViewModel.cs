using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace KetabAbee.Application.DTOs.ActiveAccount
{
    public class ActiveEmailBy5ThCodeViewModel
    {
        [DisplayName("کد فعالسازی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(5, ErrorMessage = "{0} نمیتواند بیش از {1} کاراکتر داشته باشد")]
        public string ActiveCode { get; set; }
    }
}
