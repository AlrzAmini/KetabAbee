using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KetabAbee.Application.DTOs.Admin.Products.Book;
using KetabAbee.Application.DTOs.Book;
using KetabAbee.Domain.Models.Products;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace KetabAbee.Application.Interfaces.Product
{
    public interface IProductService
    {
        #region Groups

        IEnumerable<ProductGroup> GetGroups();

        IEnumerable<ProductGroup> GetGroupsForAdmin();

        bool AddGroup(ProductGroup group);

        bool UpdateGroup(ProductGroup group);

        ProductGroup GetGroupById(int groupId);

        bool DeleteGroupById(int groupId);

        List<SelectListItem> GetGroupsForSelect();

        #endregion

        #region Book

        List<SelectListItem> GetGroupsForAddBook();

        List<SelectListItem> GetSubGroupsForAddBook(int groupId);

        bool AddBook(Book book,IFormFile imgFile);

        IEnumerable<Book> GetBooksForAdmin();

        FilterBooksViewModel GetBooksByFilter(FilterBooksViewModel filter);

        bool DeleteBook(int bookId);

        bool UpdateBook(Book book);

        Book GetBookById(int bookId);

        bool EditBook(Book book, IFormFile imgFile);

        FilterBookListViewModel GetBooksForIndex(FilterBookListViewModel filter);

        #endregion

        #region Publisher

        IEnumerable<Publisher> GetPublishers();

        bool AddPublisher(Publisher publisher);

        bool IsPublisherUnique(string publisherName);

        List<SelectListItem> GetPublishersForSelect();


        #endregion
    }
}
