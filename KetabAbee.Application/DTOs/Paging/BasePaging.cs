using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KetabAbee.Application.DTOs.Paging
{
    public class BasePaging
    {
        public BasePaging()
        {
            PageNum = 1;
            Take = Const.Paging.ItemPerPage;
            PageCountAfterAndBefor = 5;
        }

        public int PageNum { get; set; }

        public int TotalPages { get; set; }

        public int TotalEntitiesCount { get; set; }

        public int Take { get; set; }

        public int Skip { get; set; }

        public int StartPage { get; set; }

        public int EndPage { get; set; }

        public int PageCountAfterAndBefor { get; set; }

        public BasePaging GetCurrentPaging()
        {
            return this;
        }

    }
}
