using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KetabAbee.Application.DTOs.Admin.Products.Book
{
    public class ChangeInventoryViewModel
    {
        [DisplayName("تعداد افزایش موجودی")]
        [Range(0, 999999, ErrorMessage = "{0} باید بین {1} و {2} باشد")]
        public int? IncNumber { get; set; }

        [DisplayName("تعداد کاهش موجودی")]
        [Range(0, 999999, ErrorMessage = "{0} باید بین {1} و {2} باشد")]
        public int? DecNumber { get; set; }

        [DisplayName("موجودی کنونی")]
        [Range(0,999999, ErrorMessage = "{0} باید بین {1} و {2} باشد")]
        public int? CurrentInventory { get; set; }

        public string BookName { get; set; }

        public int BookId { get; set; }
    }
}
