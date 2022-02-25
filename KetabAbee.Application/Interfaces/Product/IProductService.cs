using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KetabAbee.Application.DTOs.Admin.Products.Book;
using KetabAbee.Application.DTOs.Admin.Products.Book.Publishers;
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

        IEnumerable<BookListViewModel> GetBestSellingBooks();

        bool AddBook(Book book, IFormFile imgFile);

        IEnumerable<Book> GetBooksForAdmin();

        FilterBooksViewModel GetBooksByFilter(FilterBooksViewModel filter);

        bool DeleteBook(int bookId);

        bool UpdateBook(Book book);

        Book GetBookById(int bookId);

        bool EditBook(Book book, IFormFile imgFile);

        FilterBookListViewModel GetBooksForIndex(FilterBookListViewModel filter);

        IEnumerable<BookListViewModel> GetLatestBooksInIndex(int take);

        Book GetBookForShowByBookId(int bookId);

        IEnumerable<BookListViewModel> PublisherBooks(int publisherId, Book book);

        BookListViewModel GetBookListViewModelByBook(Book book);

        ChangeInventoryViewModel GetInventoryInfoByBookId(int bookId);

        bool IncreaseInventory(ChangeInventoryViewModel inventory);

        string DecreaseInventory(ChangeInventoryViewModel inventory);

        void AddInventoryReport(int bookId, int changeId, int changeNumber, string bookName);

        IEnumerable<InventoryReport> GetBookChangeInventoryReports(int bookId);

        IEnumerable<AllInventoryReportsViewModel> GetAllInventoryReports();

        IEnumerable<BookListViewModel> GetBooksByAgeRange(string userName);

        int GetAgeByUserName(string userName);

        bool AddBookToFavorite(FavoriteBook favoriteBook);

        FavoriteBook GetFavBookInfoFromBook(int userId, int bookId);

        bool IsUserLikedBook(int userId, int bookId);

        IEnumerable<BookListViewModel> GetFavBooksByBookIds(List<int> bookIds);

        int GetFavBookIdByBookIdAndUserId(int userId, int bookId);

        bool RemoveFromFav(int likeId);

        FavoriteBook GetFavById(int likeId);

        IEnumerable<BookListViewModel> GetUserBooksForShowInUserPanel(int userId);

        IEnumerable<Book> GetBooksByName(string bookName);

        IEnumerable<string> GetBookNamesForAutoCompleteSearch(string search);

        #endregion

        #region Publisher

        IEnumerable<Publisher> GetPublishers();

        bool AddPublisher(Publisher publisher);

        bool IsPublisherUnique(string publisherName);

        List<SelectListItem> GetPublishersForSelect();

        IEnumerable<PublisherInAdminViewModel> GetPublishersForAdmin();

        int PublisherBooksCount(int publisherId);

        IEnumerable<PublisherBooksViewModel> GetPublisherBooks(int publisherId);

        bool DeletePublisher(int publisherId);

        CreatePublisherResult AddPublisherFromAdmin(CreatePublisherViewModel publisher);

        EditPublisherViewModel GetPublisherInfoForEdit(int publisherId);

        EditPublisherResult EditPublisher(EditPublisherViewModel publisher);

        IEnumerable<string> GetAllPublisherNames();

        bool IsNotPublisherNameUnique(string publisherName);

        #endregion

        #region Score

        AddScoreResult AddScore(AddBookScoreViewModel addScore);

        bool IsUserBoughtBook(int userId, int bookId);

        bool ScoreSentByUser(int userId, int bookId);

        int AllBookSentScoresCount(int bookId);

        float GetBookAverageScore(int bookId);

        int SatisfiedBookBuyersPercent(int bookId);

        #endregion
    }
}
