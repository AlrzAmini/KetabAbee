using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KetabAbee.Domain.Models.Comment.ProductComment;
using KetabAbee.Domain.Models.Order;
using KetabAbee.Domain.Models.Products;
using KetabAbee.Domain.Models.User;

namespace KetabAbee.Domain.Interfaces
{
    public interface IProductRepository
    {
        #region Group

        Task<List<ProductGroup>> GetGroups();

        IEnumerable<ProductGroup> GetGroupsForAdmin();

        bool AddGroup(ProductGroup group);

        bool UpdateGroup(ProductGroup group);

        ProductGroup GetGroupById(int groupId);

        #endregion

        #region Book

        IEnumerable<Publisher> GetPublishers();

        IEnumerable<Book> GetBooksForAdmin();

        IEnumerable<Book> GetBestSellingBooks();

        bool AddBook(Book book);

        bool UpdateBook(Book book);

        Book GetBookById(int bookId);

        IEnumerable<Book> GetLatestBook(int take);

        Book GetBookForShowByBookId(int bookId);

        IEnumerable<Book> PublisherBooks(int publisherId);

        string GetBookNameById(int bookId);

        void AddBookToFavorite(FavoriteBook favoriteBook);

        void RemoveFromFavorite(FavoriteBook favoriteBook);

        void UpdateFavorite(FavoriteBook favoriteBook);

        bool IsUserLikedBook(int userId, int bookId);

        FavoriteBook GetFavByBookIdAndUserId(int bookId, int userId, bool isLiked);

        IEnumerable<Book> GetFavBooksByBookIds(List<int> bookIds);

        int GetFavBookIdByBookIdAndUserId(int userId, int bookId);

        FavoriteBook GetFavById(int likeId);

        IEnumerable<Book> GetUserBooks(int userId);

        IEnumerable<Book> GetBooksByName(string bookName);

        IEnumerable<string> GetBookNamesForAutoCompleteSearch(string search);

        #endregion

        #region Report

        void AddInventoryReport(int bookId, int changeId, int changeNumber, string bookName);

        void UpdateInventoryReport(InventoryReport report);

        IEnumerable<InventoryReport> GetBookInventoryReports(int bookId);

        IEnumerable<InventoryReport> GetAllInventoryReports();

        #endregion

        #region Publisher

        bool AddPublisher(Publisher publisher);

        int PublisherBooksCount(int publisherId);

        IEnumerable<Book> GetPublisherBooks(int publisherId);

        Publisher GetPublisherById(int publisherId);

        bool UpdatePublisher(Publisher publisher);

        IEnumerable<string> GetAllPublisherNames();

        bool IsNotPublisherNameUnique(string publisherName);

        #endregion

        #region Score

        bool AddScore(BookScore score);

        bool IsUserBoughtBook(int userId, int bookId);

        bool ScoreSentByUser(int userId, int bookId);

        int AllBookSentScoresCount(int bookId);

        float GetBookAverageScore(int bookId);

        int SumBookQualityScores(int bookId);

        int SumBookContentScores(int bookId);

        int SatisfiedBookBuyersPercent(int bookId);

        float SumBookAverageScores(int bookId);

        #endregion

        #region Book Options

        List<int> GetBookUserIds(int bookId);

        Book GetBookByIdWithIncludes(int bookId);

        Task<List<int>> GetAllProductCommentIds(int bookId);

        Task<List<User>> GetAllBookSelectedToFavorites(int bookId);

        Task<List<BookScore>> GetAllBookScores(int bookId);

        Task<List<OrderDetail>> GetAllBookOrderDetails(int bookId);

        Task<List<Book>> GetBestSellingBooksForAdmin();

        Task<List<Book>> GetMostLikedBooksForAdmin();

        Task<List<Book>> GetBestRatedBooksForAdmin();

        #endregion

        #region compare

        User GetUserByUserName(string userName);

        void AddCompare(Compare compare);

        #endregion
    }
}
