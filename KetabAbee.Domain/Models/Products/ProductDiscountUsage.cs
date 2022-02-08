using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KetabAbee.Domain.Models.Products
{
    public class ProductDiscountUsage
    {
        [Key]
        public int ProductDiscountUsageId { get; set; }

        [Required]
        public int ProductDiscountId { get; set; }

        [Required]
        public int UserId { get; set; }

        #region props

        [Required]
        public DateTime CreateDate { get; set; }

        #endregion

        #region relations

        public ProductDiscount ProductDiscount { get; set; }

        public User.User User { get; set; }

        #endregion
    }
}
