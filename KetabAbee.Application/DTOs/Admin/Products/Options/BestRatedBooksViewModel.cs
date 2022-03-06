using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KetabAbee.Application.DTOs.Paging;

namespace KetabAbee.Application.DTOs.Admin.Products.Options
{
    public class BestRatedBooksViewModel
    {
        public int BookId { get; set; }

        public string BookName { get; set; }

        public string ImageName { get; set; }

        public float AverageScore { get; set; }

        public int ContentScore { get; set; }

        public int QualityScore { get; set; }
    }

    public class BestRatedBooksWithPaging : BasePaging
    {
        public List<BestRatedBooksViewModel> Books { get; set; }

        #region Methods

        public BestRatedBooksWithPaging SetBooks(List<BestRatedBooksViewModel> books)
        {
            Books = books;
            return this;
        }

        public BestRatedBooksWithPaging SetPaging(BasePaging paging)
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
