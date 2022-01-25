using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KetabAbee.Domain.Models.Products
{
    public class InventoryReport
    {
        [Key]
        public int ReportId { get; set; }

        [Required]
        public int BookId { get; set; }

        [Required]
        [MaxLength(300)]
        public string BookName { get; set; }

        [Required]
        public int ChangeId { get; set; } // change Id : 1 -> increase // change Id : 2 -> Decrease

        [Required]
        public int ChangeNumber { get; set; }

        public DateTime Date { get; set; }

        
    }
}
