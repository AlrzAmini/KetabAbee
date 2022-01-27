using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KetabAbee.Data.Context;
using KetabAbee.Domain.Interfaces;
using KetabAbee.Domain.Models.Products;
using Microsoft.EntityFrameworkCore;

namespace KetabAbee.Data.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly KetabAbeeDBContext _context;

        public ProductRepository(KetabAbeeDBContext context)
        {
            _context = context;
        }

        public bool AddGroup(ProductGroup group)
        {
            _context.ProductGroups.Add(group);
            _context.SaveChanges();
            return true;
        }

        public IEnumerable<ProductGroup> GetGroups()
        {
            return _context.ProductGroups;
        }

        public IEnumerable<ProductGroup> GetGroupsForAdmin()
        {
            return _context.ProductGroups
                .Include(g => g.ProductGroups);
        }

        public bool UpdateGroup(ProductGroup group)
        {
            _context.ProductGroups.Update(group);
            _context.SaveChanges();
            return true;
        }

        public ProductGroup GetGroupById(int groupId)
        {
            return _context.ProductGroups.Find(groupId);
        }

        public IEnumerable<Publisher> GetPublishers()
        {
            return _context.Publishers;
        }

        public bool AddBook(Book book)
        {
            _context.Books.Add(book);
            _context.SaveChanges();
            return true;
        }

        public IEnumerable<Book> GetBooksForAdmin()
        {
            return _context.Books
                .Include(b => b.Publisher)
                .Include(b => b.Group)
                .Include(b => b.SubGroup)
                .Include(b => b.SubGroup2);
        }

        public bool AddPublisher(Publisher publisher)
        {
            _context.Publishers.Add(publisher);
            _context.SaveChanges();
            return true;
        }

        public bool UpdateBook(Book book)
        {
            _context.Books.Update(book);
            _context.SaveChanges();
            return true;
        }

        public Book GetBookById(int bookId)
        {
            return _context.Books.Find(bookId);
        }

        public IEnumerable<Book> GetLatestBook(int take)
        {
            return _context.Books
                .Include(b => b.Publisher)
                .OrderByDescending(b => b.BookId)
                .Take(take);
        }

        public Book GetBookForShowByBookId(int bookId)
        {
            return _context.Books
                .Include(b => b.Publisher)
                .Include(b => b.Group)
                .Include(b => b.SubGroup)
                .Include(b => b.SubGroup2)
                .SingleOrDefault(b => b.BookId == bookId);
        }

        public IEnumerable<Book> PublisherBooks(int publisherId)
        {
            return _context.Books.Include(b => b.Publisher)
                .Where(b => b.PublisherId == publisherId);
        }

        public void AddInventoryReport(int bookId, int changeId, int changeNumber, string bookName)
        {
            var report = new InventoryReport
            {
                BookId = bookId,
                ChangeId = changeId,
                ChangeNumber = changeNumber,
                Date = DateTime.Now,
                BookName = bookName
            };
            _context.InventoryReports.Add(report);
            _context.SaveChanges();
        }

        public void UpdateInventoryReport(InventoryReport report)
        {
            _context.InventoryReports.Update(report);
            _context.SaveChanges();
        }

        public IEnumerable<InventoryReport> GetBookInventoryReports(int bookId)
        {
            return _context.InventoryReports.Where(r => r.BookId == bookId);
        }

        public IEnumerable<InventoryReport> GetAllInventoryReports()
        {
            return _context.InventoryReports;
        }

        public string GetBookNameById(int bookId)
        {
            return GetBookById(bookId).Name;
        }

        public int GetAgeByUserName(string userName)
        {
            return (int)_context.Users.SingleOrDefault(u => u.UserName == userName).Age;
        }

        public void AddBookToFavorite(FavoriteBook favoriteBook)
        {
            _context.FavoriteBooks.Add(favoriteBook);
            _context.SaveChanges();
        }

        public void RemoveFromFavorite(FavoriteBook favoriteBook)
        {
            _context.FavoriteBooks.Update(favoriteBook);
            _context.SaveChanges();
        }

        public bool IsUserLikedBook(int userId, int bookId)
        {
            return _context.FavoriteBooks.Any(f => f.UserId == userId && f.BookId == bookId && f.IsLiked);
        }

        public void UpdateFavorite(FavoriteBook favoriteBook)
        {
            _context.FavoriteBooks.Update(favoriteBook);
            _context.SaveChanges();
        }

        public FavoriteBook GetFavByBookIdAndUserId(int bookId, int userId, bool isLiked)
        {
            return _context.FavoriteBooks.SingleOrDefault(f => f.BookId == bookId && f.UserId == userId);
        }

        public IEnumerable<Book> GetFavBooksByBookIds(List<int> bookIds)
        {
            return bookIds.Select(GetBookById);
        }

        public int GetFavBookIdByBookIdAndUserId(int userId, int bookId)
        {
            return _context.FavoriteBooks.FirstOrDefault(f => f.UserId == userId && f.BookId == bookId).LikeId;
        }

        public FavoriteBook GetFavById(int likeId)
        {
            return _context.FavoriteBooks.Find(likeId);
        }
    }
}
