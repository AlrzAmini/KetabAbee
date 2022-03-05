using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KetabAbee.Application.DTOs.Admin.Products.Options
{
    public class BookOrderViewModel
    {
        public int DetailId { get; set; }

        public int OrderId { get; set; }

        public int ProductId { get; set; }

        public string ProductName { get; set; }

        public string UserName { get; set; }

        public int UserId { get; set; }

        public DateTime CreateDate { get; set; }

        public int Price { get; set; }

        public int Count { get; set; }

        public bool IsFinally { get; set; }
    }
}
