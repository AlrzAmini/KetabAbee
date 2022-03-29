using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KetabAbee.Data.Context;
using KetabAbee.Domain.Interfaces;
using KetabAbee.Domain.Models.Order;
using KetabAbee.Domain.Models.Products;
using KetabAbee.Domain.Models.User;
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

        public async Task<List<ProductGroup>> GetGroups()
        {
            return await _context.ProductGroups.ToListAsync();
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
                .Include(b => b.SubGroup2)
                .OrderByDescending(b => b.BookId);
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
            return _context.Books.Include(b => b.ProductDiscounts)
                .FirstOrDefault(b => b.BookId == bookId);
        }

        public async Task<List<Book>> GetLatestBook(int take)
        {
            return await _context.Books
                .Include(b => b.Publisher)
                .OrderByDescending(b => b.BookId)
                .Take(take).ToListAsync();
        }

        public async Task<Book> GetBookForShowByBookId(int bookId)
        {
            return await _context.Books
                .Include(b => b.Publisher)
                .Include(b => b.Group)
                .Include(b => b.SubGroup)
                .Include(b => b.SubGroup2)
                .SingleOrDefaultAsync(b => b.BookId == bookId);
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

        public FavoriteBook GetFavBookByBookIdAndUserId(int userId, int bookId)
        {
            return _context.FavoriteBooks.FirstOrDefault(f => f.UserId == userId && f.BookId == bookId);
        }

        public FavoriteBook GetFavById(int likeId)
        {
            return _context.FavoriteBooks.Find(likeId);
        }

        public bool AddScore(BookScore score)
        {
            try
            {
                _context.BookScores.Add(score);
                _context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool IsUserBoughtBook(int userId, int bookId)
        {
            return _context.UserBooks.Any(s => s.UserId == userId && s.BookId == bookId);
        }

        public bool ScoreSentByUser(int userId, int bookId)
        {
            return _context.BookScores.Any(s => s.UserId == userId && s.BookId == bookId && s.IsScored);
        }

        public int AllBookSentScoresCount(int bookId)
        {
            return _context.BookScores.Count(s => s.BookId == bookId);
        }

        public float GetBookAverageScore(int bookId)
        {
            try
            {
                var bookScoresCount = AllBookSentScoresCount(bookId);
                if (bookScoresCount != 0)
                {
                    return (float)Math.Round(SumBookAverageScores(bookId) / AllBookSentScoresCount(bookId), 2);
                }

                return default;
            }
            catch
            {
                return default;
            }
        }

        public int SumBookQualityScores(int bookId)
        {
            return _context.BookScores.Where(s => s.BookId == bookId).Sum(s => s.QualityScore);
        }

        public int SumBookContentScores(int bookId)
        {
            return _context.BookScores.Where(s => s.BookId == bookId).Sum(s => s.ContentScore);
        }

        public int SatisfiedBookBuyersPercent(int bookId)
        {
            // when i try to send this data to **service** i ve got error about memory leak
            try
            {
                var allBookScoresCount = _context.BookScores.Count(s => s.BookId == bookId);
                if (allBookScoresCount == 0)
                {
                    return default;
                }
                var satisfiedBookScoresCount = _context.BookScores.Count(s => s.BookId == bookId && s.AverageScores >= 3);
                return satisfiedBookScoresCount * (100 / allBookScoresCount);
            }
            catch
            {
                return default;
            }
        }

        public float SumBookAverageScores(int bookId)
        {
            return _context.BookScores.Where(s => s.BookId == bookId).Sum(s => s.AverageScores);
        }

        public IEnumerable<Book> GetUserBooks(int userId)
        {
            return _context.UserBooks.Include(u => u.Book)
                .Where(u => u.UserId == userId)
                .Select(s => s.Book)
                .Distinct();
        }

        public IEnumerable<Book> GetBooksByName(string bookName)
        {
            return _context.Books.Where(b => EF.Functions.Like(b.Name, $"%{bookName}%"));
        }

        public IEnumerable<Book> GetBestSellingBooks()
        {
            return _context.Books.Include(b => b.Publisher)
                .Include(b => b.OrderDetails)
                .Where(b => b.OrderDetails.Any() && b.OrderDetails.Count > 3)
                .OrderByDescending(b => b.OrderDetails.Sum(d => d.Count))
                .Take(12);
        }

        public IEnumerable<string> GetBookNamesForAutoCompleteSearch(string search)
        {
            return _context.Books
                .Where(b => EF.Functions.Like(b.Name, $"%{search}%"))
                .Select(b => b.Name);
        }

        public int PublisherBooksCount(int publisherId)
        {
            return _context.Books.Count(b => b.PublisherId == publisherId);
        }

        public IEnumerable<Book> GetPublisherBooks(int publisherId)
        {
            return _context.Books.Include(p => p.Publisher).Where(b => b.PublisherId == publisherId);
        }

        public Publisher GetPublisherById(int publisherId)
        {
            return _context.Publishers.Find(publisherId);
        }

        public bool UpdatePublisher(Publisher publisher)
        {
            try
            {
                _context.Publishers.Update(publisher);
                _context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public IEnumerable<string> GetAllPublisherNames()
        {
            return _context.Publishers.Select(p => p.PublisherName);
        }

        public bool IsNotPublisherNameUnique(string publisherName)
        {
            return GetAllPublisherNames().Any(n => n == publisherName);
        }

        public List<int> GetBookUserIds(int bookId)
        {
            return _context.UserBooks
                .Where(ub => ub.BookId == bookId)
                .Select(ub => ub.UserId)
                .Distinct()
                .ToList();
        }

        public Book GetBookByIdWithIncludes(int bookId)
        {
            return _context.Books
                .Include(b => b.Group)
                .Include(b => b.SubGroup)
                .Include(b => b.SubGroup2)
                .Include(b => b.Publisher)
                .FirstOrDefault(b => b.BookId == bookId);
        }

        public async Task<List<int>> GetAllProductCommentIds(int bookId)
        {
            return await _context.ProductComments
                .Where(c => c.ProductId == bookId)
                .Select(c => c.CommentId).ToListAsync();
        }

        public async Task<List<User>> GetAllBookSelectedToFavorites(int bookId)
        {
            return await _context.FavoriteBooks
                .Include(f => f.User)
                .Where(f => f.BookId == bookId)
                .Select(f => f.User).ToListAsync();
        }

        public async Task<List<BookScore>> GetAllBookScores(int bookId)
        {
            return await _context.BookScores
                .Include(s => s.User)
                .Include(s => s.Book)
                .Where(s => s.BookId == bookId)
                .ToListAsync();
        }

        public async Task<List<OrderDetail>> GetAllBookOrderDetails(int bookId)
        {
            return await _context.OrderDetails
                .Include(d => d.Order)
                .ThenInclude(o => o.User)
                .Include(d => d.Product)
                .Where(d => d.ProductId == bookId)
                .OrderByDescending(d => d.DetailId)
                .ToListAsync();
        }

        public async Task<List<Book>> GetBestSellingBooksForAdmin()
        {
            return await _context.Books
                .Include(b => b.OrderDetails)
                .Where(b => b.OrderDetails.Any())
                .OrderByDescending(b => b.OrderDetails.Sum(d => d.Count))
                .ToListAsync();
        }

        public async Task<List<Book>> GetMostLikedBooksForAdmin()
        {
            return await _context.Books
                .Include(b => b.FavoriteBook)
                .Where(b => b.FavoriteBook.Any())
                .OrderByDescending(b => b.FavoriteBook.Count(f => f.IsLiked))
                .ToListAsync();
        }

        public async Task<List<Book>> GetBestRatedBooksForAdmin()
        {
            return await _context.Books
                .Include(b => b.BookScores)
                .Where(b => b.BookScores.Any())
                .OrderByDescending(b => b.BookScores.Sum(s => s.AverageScores))
                .ToListAsync();
        }

        public User GetUserByUserName(string userName)
        {
            return _context.Users.FirstOrDefault(u => u.UserName == userName);
        }

        public FavoriteBook GetFavBookByBookAndUserId(int bookId, int userId)
        {
            return _context.FavoriteBooks.FirstOrDefault(f => f.UserId == userId && f.BookId == bookId);
        }

        public async Task<Compare> GetUnFullCompareByUserIp(string userIp)
        {
            return await _context.Compares
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.UserIp == userIp && !c.IsFull);
        }

        public async Task<Compare> GetUnFullCompareByUserId(int userId)
        {
            return await _context.Compares
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.UserId == userId && !c.IsFull);
        }

        public async Task<bool> AddCompare(Compare compare)
        {
            try
            {
                await _context.Compares.AddAsync(compare);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<CompareItem> GetCompareItemByCompareAndBookId(string compareId, int bookId)
        {
            return await _context.CompareItems
                .FirstOrDefaultAsync(i => i.CompareId == compareId && i.BookId == bookId);
        }

        public async Task<bool> AddCompareItem(CompareItem item)
        {
            try
            {
                await _context.CompareItems.AddAsync(item);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<Compare> GetCompareById(string compareId)
        {
            return await _context.Compares
                .Include(c => c.Items)
                .ThenInclude(i => i.Book)
                .ThenInclude(b => b.Publisher)
                .Include(i => i.Items)
                .ThenInclude(i => i.Book)
                .ThenInclude(b => b.Group)
                .FirstOrDefaultAsync(c => c.CompareId == compareId);
        }

        public async Task<bool> UpdateCompare(Compare compare)
        {
            try
            {
                _context.Compares.Update(compare);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> RemoveItemFromCompare(CompareItem compareItem)
        {
            var compare = await GetCompareById(compareItem.CompareId);
            compare.IsFull = false;
            await UpdateCompare(compare);
            compareItem.IsDelete = true;
            return await UpdateCompareItem(compareItem);
        }

        public async Task<bool> UpdateCompareItem(CompareItem compareItem)
        {
            try
            {
                _context.CompareItems.Update(compareItem);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<IQueryable<Compare>> GetAllUserIpCompares(string userIp)
        {
            return await Task.FromResult(_context.Compares
                .Include(c => c.Items)
                .Where(c => c.UserIp == userIp)
                .OrderByDescending(c => c.CompareDate));
        }

        public async Task<IQueryable<Compare>> GetAllUserIdCompares(int userId)
        {
            return await Task.FromResult(_context.Compares
                .Include(c => c.Items)
                .Where(c => c.UserId == userId)
                .OrderByDescending(c => c.CompareDate));
        }
    }
}
