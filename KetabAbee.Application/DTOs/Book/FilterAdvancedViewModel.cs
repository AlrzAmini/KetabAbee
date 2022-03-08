using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KetabAbee.Application.DTOs.Paging;
using KetabAbee.Domain.Models.Products;

namespace KetabAbee.Application.DTOs.Book
{
    public class FilterAdvancedViewModel : BasePaging
    {
        #region Props

        public string BookName { get; set; }

        public int? PublisherId { get; set; }

        [DisplayName("شروع قیمت")]
        [Range(0, 1000000, ErrorMessage = "{0} باید بین {1} و {2} باشد")]
        public int? MinPrice { get; set; }

        [DisplayName("پایان قیمت")]
        [Range(1000, 1000000, ErrorMessage = "{0} باید بین {1} و {2} باشد")]
        public int? MaxPrice { get; set; }

        public string Writer { get; set; }

        public int? MinPageCount { get; set; }

        public int? MaxPageCount { get; set; }

        public AgeRange? AgeRange { get; set; }

        public CoverType? CoverType { get; set; }

        public bool IsExist { get; set; }

        public int ResultCount { get; set; } = 0;

        public List<BookListViewModel> Books { get; set; }

        #endregion

        #region Methods

        public FilterAdvancedViewModel SetBooks(List<BookListViewModel> books)
        {
            Books = books;
            return this;
        }

        public FilterAdvancedViewModel SetPaging(BasePaging paging)
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
