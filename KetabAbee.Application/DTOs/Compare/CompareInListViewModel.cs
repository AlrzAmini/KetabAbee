using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KetabAbee.Application.DTOs.Compare
{
    public class CompareInListViewModel
    {
        public string CompareId { get; set; }

        public DateTime CompareDate { get; set; }

        public bool IsFull { get; set; }

        public int ItemsCount { get; set; }
    }
}
