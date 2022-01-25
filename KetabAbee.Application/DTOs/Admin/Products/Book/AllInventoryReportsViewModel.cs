using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KetabAbee.Application.DTOs.Admin.Products.Book
{
    public class AllInventoryReportsViewModel
    {
        public int ReportId { get; set; }


        public string BookName { get; set; }


        public int ChangeId { get; set; } // change Id : 1 -> increase // change Id : 2 -> Decrease


        public int ChangeNumber { get; set; }

        public DateTime Date { get; set; }
    }
}
