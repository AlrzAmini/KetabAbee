using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KetabAbee.Application.DTOs.Paging;

namespace KetabAbee.Application.DTOs.Admin.Products.Options
{
    public class MostLikedBooksViewModel
    {
        public int BookId { get; set; }

        public string ImageName { get; set; }

        public string BookName { get; set; }

        public int LikesCount { get; set; }
    }

    public class MostLikedBooksViewModelWithPaging : BasePaging
    {
        public List<MostLikedBooksViewModel> Books { get; set; }

        #region Methods

        public MostLikedBooksViewModelWithPaging SetBooks(List<MostLikedBooksViewModel> books)
        {
            Books = books;
            return this;
        }

        public MostLikedBooksViewModelWithPaging SetPaging(BasePaging paging)
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
