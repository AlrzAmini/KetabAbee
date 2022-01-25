using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KetabAbee.Application.DTOs.Admin.User;
using KetabAbee.Application.DTOs.Paging;
using KetabAbee.Domain.Models.Products;

namespace KetabAbee.Application.DTOs.Admin.Products.Book
{
    public class FilterBooksViewModel : BasePaging
    {
        #region Props

        public int? MinNumber { get; set; }

        public int? MaxNumber { get; set; }

        public string BookName { get; set; }

        public string PublisherName { get; set; }

        public int? MinPrice { get; set; }

        public int? MaxPrice { get; set; }

        public string Writer { get; set; }

        public int? MinPageCount { get; set; }

        public int? MaxPageCount { get; set; }

        public AgeRange? AgeRange { get; set; }

        public CoverType? CoverType { get; set; }

        public List<Domain.Models.Products.Book> Books { get; set; }

        #endregion

        #region Methods

        public FilterBooksViewModel SetBooks(List<Domain.Models.Products.Book> books)
        {
            Books = books;
            return this;
        }

        public FilterBooksViewModel SetPaging(BasePaging paging)
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
