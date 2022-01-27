using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KetabAbee.Application.DTOs.Paging;
using KetabAbee.Domain.Models.Products;

namespace KetabAbee.Application.DTOs.Book
{
    public class FilterBookListViewModel : BasePaging
    {
        #region props

        public string BookName { get; set; }

        public string PublisherName { get; set; }

        public int? MinPrice { get; set; }

        public int? MaxPrice { get; set; }

        public string Writer { get; set; }

        public List<int> SearchCategory { get; set; } 

        public List<int> SelectedPublishers { get; set; } 

        public AgeRange? AgeRange { get; set; }

        public List<BookListViewModel> Books { get; set; }

        public bool Exist { get; set; }

        #endregion

        #region Method

        public FilterBookListViewModel SetBooks(List<BookListViewModel> books)
        {
            Books = books;
            return this;
        }

        public FilterBookListViewModel SetPaging(BasePaging paging)
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
