using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KetabAbee.Application.DTOs.Paging;

namespace KetabAbee.Application.DTOs.Admin.Products.Options
{
    public class BestSellingsWithPagingViewModel : BasePaging
    {
        public List<BestSellingBookViewModel> Books { get; set; }

        #region Methods

        public BestSellingsWithPagingViewModel SetBooks(List<BestSellingBookViewModel> books)
        {
            Books = books;
            return this;
        }

        public BestSellingsWithPagingViewModel SetPaging(BasePaging paging)
        {
            PageNum = paging.PageNum;
            TotalEntitiesCount = paging.TotalEntitiesCount;
            StartPage = paging.StartPage;
            EndPage = paging.EndPage;
            PageCountAfterAndBefor = paging.PageCountAfterAndBefor;
            Take = paging.Take;
            Skip = paging.Skip;
            TotalPages = paging.TotalPages;

            return this;
        }

        #endregion
    }
}
