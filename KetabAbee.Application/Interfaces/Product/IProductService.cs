using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KetabAbee.Application.DTOs.Admin.Products.Book;
using KetabAbee.Application.DTOs.Admin.Products.Book.Publishers;
using KetabAbee.Application.DTOs.Admin.Products.Options;
using KetabAbee.Application.DTOs.Book;
using KetabAbee.Application.DTOs.Compare;
using KetabAbee.Domain.Models.Products;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace KetabAbee.Application.Interfaces.Product
{
    public interface IProductService
    {
        #region Groups

        Task<List<ProductGroup>> GetGroups();

        IEnumerable<ProductGroup> GetGroupsForAdmin();

        bool AddGroup(ProductGroup group);

        bool UpdateGroup(ProductGroup group);

        ProductGroup GetGroupById(int groupId);

        bool DeleteGroupById(int groupId);

        List<SelectListItem> GetGroupsForSelect();

        #endregion

        #region Book

        Task<List<SelectListItem>> GetGroupsForAddBook();

        Task<List<SelectListItem>> GetSubGroupsForAddBook(int groupId);

        IEnumerable<BookListViewModel> GetBestSellingBooks(int? userId);

        bool AddBook(Book book, IFormFile imgFile);

        IEnumerable<Book> GetBooksForAdmin();

        FilterBooksViewModel GetBooksByFilter(FilterBooksViewModel filter);

        bool DeleteBook(int bookId);

        bool UpdateBook(Book book);

        Book GetBookById(int bookId);

        bool EditBook(Book book, IFormFile imgFile);

        FilterBookListViewModel GetBooksForIndex(FilterBookListViewModel filter, int? userId);

        Task<List<BookListViewModel>> GetLatestBooksInIndex(int take, int? userId);

        Task<Book> GetBookForShowByBookId(int bookId);

        IEnumerable<BookListViewModel> PublisherBooks(int publisherId, Book book, int? userId);

        BookListViewModel GetBookListViewModelByBook(Book book);

        ChangeInventoryViewModel GetInventoryInfoByBookId(int bookId);

        bool IncreaseInventory(ChangeInventoryViewModel inventory);

        string DecreaseInventory(ChangeInventoryViewModel inventory);

        void AddInventoryReport(int bookId, int changeId, int changeNumber, string bookName);

        IEnumerable<InventoryReport> GetBookChangeInventoryReports(int bookId);

        IEnumerable<AllInventoryReportsViewModel> GetAllInventoryReports();

        IEnumerable<BookListViewModel> GetBooksByAgeRange(string userName, int? userId);

        int? GetAgeByUserName(string userName);

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

        Task<FilterAdvancedViewModel> FilterBooksForFilterAdvanced(FilterAdvancedViewModel filter);

        AddBookToFavoriteResult AddBookToFavoriteById(int bookId, int userId);

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

        #region Book Options

        List<BookUsersViewModel> GetBookUsers(int bookId);

        ShowBookInfoViewModel GetBookForShowInAdminBookInfo(int bookId);

        int GetLotteryWinner(int bookId);

        Task<DeleteProductCommentsResult> DeleteAllProductComments(int bookId);

        IEnumerable<ProductCommentForShowInAdminBookInfoViewModel> GetProductCommentsForAdminInBookInfo(int bookId);

        Task<List<BookSelectedToFavoriteUsersViewModel>> GetAllBookSelectedToFavorites(int bookId);

        Task<List<BookScoreViewModel>> GetAllBookScores(int bookId);

        Task<List<BookOrderViewModel>> GetAllBookOrderDetails(int bookId);

        Task<BestSellingsWithPagingViewModel> GetBestSellingBooksForAdmin(BestSellingsWithPagingViewModel model);

        Task<MostLikedBooksViewModelWithPaging> GetMostLikedBooksForAdmin(MostLikedBooksViewModelWithPaging model);

        Task<BestRatedBooksWithPaging> GetBestRatedBooksForAdmin(BestRatedBooksWithPaging model);

        #endregion

        #region Compare

        Task<string> AddCompare(string userIp, int bookId, int? userId);

        Task<ShowCompareToUserViewModel> GetCompareForShow(string compareId, string userIp, int? userId);

        System.Threading.Tasks.Task SetFullToCompare(string compareId);

        Task<bool> RemoveItemFromCompare(int bookId, string compareId);

        Task<List<CompareInListViewModel>> GetAllUserIpCompares(string userIp);

        Task<List<CompareInListViewModel>> GetAllUserIdCompares(int userId);

        Task<bool> IsCompareLastRecord(string compareId, string userIp, int? userId);

        #endregion
    }
}
