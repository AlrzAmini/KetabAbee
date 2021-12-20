using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KetabAbee.Application.DTOs.Paging
{
    public class Pager
    {
        public static BasePaging Build(int pageNum, int totalEntitiesCount, int take, int pageCountAfterAndBefor)
        {
            var totalPages = (int)Math.Ceiling((decimal)totalEntitiesCount / take);

            return new BasePaging()
            {
                PageNum = pageNum,
                TotalEntitiesCount = totalEntitiesCount,
                Take = take,
                Skip = (pageNum - 1) * take,
                StartPage = pageNum - pageCountAfterAndBefor <= 0 ? 1 : pageNum - pageCountAfterAndBefor,
                EndPage = pageNum + pageCountAfterAndBefor > totalPages ? totalPages : pageNum + pageCountAfterAndBefor,
                PageCountAfterAndBefor = pageCountAfterAndBefor,
                TotalPages = totalPages
            };
        }
    }
}
