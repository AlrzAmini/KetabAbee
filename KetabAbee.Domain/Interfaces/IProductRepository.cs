using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KetabAbee.Domain.Models.Products;

namespace KetabAbee.Domain.Interfaces
{
    public interface IProductRepository
    {
        #region Group

        IEnumerable<ProductGroup> GetGroups();

        IEnumerable<ProductGroup> GetGroupsForAdmin();

        bool AddGroup(ProductGroup group);

        bool UpdateGroup(ProductGroup group);

        ProductGroup GetGroupById(int groupId);

        #endregion

        #region Book

        IEnumerable<Publisher> GetPublishers();

        IEnumerable<Book> GetBooksForAdmin();

        bool AddBook(Book book);

        bool UpdateBook(Book book);

        Book GetBookById(int bookId);

        IEnumerable<Book> GetLatestBook(int take);

        Book GetBookForShowByBookId(int bookId);

        IEnumerable<Book> PublisherBooks(int publisherId);

        string GetBookNameById(int bookId);

        #endregion

        #region Report

        void AddInventoryReport(int bookId, int changeId, int changeNumber, string bookName);

        void UpdateInventoryReport(InventoryReport report);

        IEnumerable<InventoryReport> GetBookInventoryReports(int bookId);

        IEnumerable<InventoryReport> GetAllInventoryReports();

        #endregion

        #region Publisher

        bool AddPublisher(Publisher publisher);


        #endregion
    }
}
