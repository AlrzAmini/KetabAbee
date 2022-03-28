using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KetabAbee.Application.DTOs.Compare
{
    public class ShowCompareToUserViewModel
    {
        public List<Domain.Models.Products.Book> Items { get; set; }

        public DateTime CreateDate { get; set; }

        public string CompareId { get; set; }

        public bool IsCompareLastRecord { get; set; }
    }
}
