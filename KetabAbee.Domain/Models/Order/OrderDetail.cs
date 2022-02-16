using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KetabAbee.Domain.Models.Products;

namespace KetabAbee.Domain.Models.Order
{
    public class OrderDetail
    {
        [Key]
        public int DetailId { get; set; }

        [Required]
        public int OrderId { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Required]
        public int Price { get; set; }

        [Required]
        public int Count { get; set; }

        public bool IsDelete { get; set; }


        #region relations

        public Order Order { get; set; }

        public Book Product { get; set; }

        #endregion
    }

    public enum ChangeCountResult
    {
        Success,
        OutOfRange,
        NotFound,
        UnHandledException
    }
}
