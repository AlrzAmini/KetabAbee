using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KetabAbee.Domain.Models.Products
{
    public class ProductDiscount
    {
        [Key]
        public int DiscountId { get; set; }

        [Required]
        public int ProductId { get; set; }

        #region props

        [DisplayName("درصد تخفیف")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [Range(1, 100, ErrorMessage = "{0} باید بین {1} و {2} باشد")]
        public int Percent { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime ExpiredDate { get; set; }

        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public int DiscountNumber { get; set; }

        public bool IsDelete { get; set; }

        #endregion

        #region relations

        public Book Product { get; set; }

        public ICollection<ProductDiscountUsage> ProductDiscountUsages { get; set; }

        #endregion
    }
}
