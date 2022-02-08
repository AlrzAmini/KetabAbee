using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KetabAbee.Application.DTOs.Discount
{
    public class CreateDiscountViewModel
    {
        #region props

        public int productId { get; set; }

        [DisplayName("درصد تخفیف")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [Range(1, 100, ErrorMessage = "{0} باید بین {1} و {2} باشد")]
        public int Percent { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [DisplayName("تاریخ انقضا")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public string ExpiredDate { get; set; }

        [DisplayName("تعداد")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public int DiscountNumber { get; set; }

        #endregion
    }

    public enum CreateDiscountResult
    {
        Success,
        NotFound,
        Error
    }
}
