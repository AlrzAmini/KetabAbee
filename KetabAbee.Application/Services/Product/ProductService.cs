using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KetabAbee.Application.Const;
using KetabAbee.Application.Convertors;
using KetabAbee.Application.DTOs.Admin.Products.Book;
using KetabAbee.Application.DTOs.Admin.Products.Book.Publishers;
using KetabAbee.Application.DTOs.Admin.Products.Options;
using KetabAbee.Application.DTOs.Book;
using KetabAbee.Application.DTOs.Paging;
using KetabAbee.Application.Generators;
using KetabAbee.Application.Interfaces.Product;
using KetabAbee.Application.Interfaces.User;
using KetabAbee.Application.Security;
using KetabAbee.Domain.Interfaces;
using KetabAbee.Domain.Models.Products;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace KetabAbee.Application.Services.Product
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IUserRepository _userRepository;
        private readonly ICommentRepository _commentRepository;

        public ProductService(IProductRepository productRepository, IUserRepository userRepository, ICommentRepository commentRepository)
        {
            _productRepository = productRepository;
            _userRepository = userRepository;
            _commentRepository = commentRepository;
        }

        public bool AddGroup(ProductGroup group)
        {
            return _productRepository.AddGroup(group);
        }

        public async Task<List<ProductGroup>> GetGroups()
        {
            return await _productRepository.GetGroups();
        }

        public IEnumerable<ProductGroup> GetGroupsForAdmin()
        {
            return _productRepository.GetGroupsForAdmin();
        }

        public bool UpdateGroup(ProductGroup group)
        {
            return _productRepository.UpdateGroup(group);
        }

        public ProductGroup GetGroupById(int groupId)
        {
            return _productRepository.GetGroupById(groupId);
        }

        public bool DeleteGroupById(int groupId)
        {
            var group = GetGroupById(groupId);
            group.IsDelete = true;
            return UpdateGroup(group);
        }

        public BookListViewModel GetBookListViewModelByBook(Book book)
        {
            return new BookListViewModel
            {
                BookId = book.BookId,
                ImageName = book.ImageName,
                PublisherName = book.Publisher.PublisherName,
                Name = book.Name,
                Price = book.Price,
                Writer = book.Writer
            };
        }

        public IEnumerable<Publisher> GetPublishers()
        {
            return _productRepository.GetPublishers();
        }

        public async Task<List<SelectListItem>> GetGroupsForAddBook()
        {
            var groups = await _productRepository.GetGroups();

            return groups.Where(g => g.ParentId == null)
             .Select(g => new SelectListItem
             {
                 Value = g.GroupId.ToString(),
                 Text = g.GroupTitle
             }).ToList();
        }

        public async Task<List<SelectListItem>> GetSubGroupsForAddBook(int groupId)
        {
            var groups = await _productRepository.GetGroups();

            return groups.Where(g => g.ParentId == groupId)
             .Select(g => new SelectListItem
             {
                 Value = g.GroupId.ToString(),
                 Text = g.GroupTitle
             }).ToList();
        }

        public bool AddBook(Book book, IFormFile imgFile)
        {

            if (imgFile != null)
            {
                // add image
                // generate name
                book.ImageName = CodeGenerator.GenerateUniqCode() + Path.GetExtension(imgFile.FileName);

                // image path
                string imgPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/Book/image", book.ImageName);

                // save file in file
                using (var stream = new FileStream(imgPath, FileMode.Create))
                {
                    imgFile.CopyTo(stream);
                }

                // thumb
                var imgResizer = new ImageConvertor();
                string imgThumbPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/Book/thumb", book.ImageName);
                imgResizer.Image_resize(imgPath, imgThumbPath, 400);
            }
            else
            {
                book.ImageName = "Defualt.jpg";
            }

            return _productRepository.AddBook(book);
        }

        public IEnumerable<Book> GetBooksForAdmin()
        {
            return _productRepository.GetBooksForAdmin();
        }

        public FilterBooksViewModel GetBooksByFilter(FilterBooksViewModel filter)
        {
            var result = _productRepository.GetBooksForAdmin().AsQueryable();

            // filter by book id range
            #region Id

            if (filter.MinNumber != null)
            {
                result = result.Where(r => r.BookId >= filter.MinNumber);
            }
            if (filter.MaxNumber != null)
            {
                result = result.Where(r => r.BookId <= filter.MaxNumber);
            }

            #endregion

            // filter by price range
            #region price

            if (filter.MinPrice != null)
            {
                result = result.Where(r => r.Price >= filter.MinPrice);
            }
            if (filter.MaxPrice != null)
            {
                result = result.Where(r => r.Price <= filter.MaxPrice);
            }

            #endregion

            // filter by page count range
            #region page

            if (filter.MinPageCount != null)
            {
                result = result.Where(r => r.PagesCount >= filter.MinPageCount);
            }
            if (filter.MaxPageCount != null)
            {
                result = result.Where(r => r.PagesCount <= filter.MaxPageCount);
            }

            #endregion

            // filter by book Name
            #region book name

            if (!string.IsNullOrEmpty(filter.BookName))
            {
                result = result.Where(r => r.Name.Contains(filter.BookName));
            }

            #endregion

            // filter by publisher name
            #region publisher

            if (!string.IsNullOrEmpty(filter.PublisherName))
            {
                result = result.Where(r => r.Publisher.PublisherName.Contains(filter.PublisherName));
            }

            #endregion

            // filter by writer name
            #region writer

            if (!string.IsNullOrEmpty(filter.Writer))
            {
                result = result.Where(r => r.Writer.Contains(filter.Writer));
            }

            #endregion

            // filter by age range
            #region age

            if (filter.AgeRange != null)
            {
                result = result.Where(r => r.AgeRange == filter.AgeRange.Value);
            }

            #endregion

            // filter by cover type
            #region cover

            if (filter.CoverType != null)
            {
                result = result.Where(r => r.CoverType == filter.CoverType.Value);
            }

            #endregion

            //paging
            var pager = Pager.Build(filter.PageNum, result.Count(), filter.Take, filter.PageCountAfterAndBefor);
            var books = result.Paging(pager).ToList();


            return filter.SetPaging(pager).SetBooks(books);
        }

        public bool AddPublisher(Publisher publisher)
        {
            return _productRepository.AddPublisher(publisher);
        }

        public bool IsPublisherUnique(string publisherName)
        {
            return GetPublishers().Any(p => p.PublisherName == publisherName);
        }

        public bool DeleteBook(int bookId)
        {
            try
            {
                var book = GetBookById(bookId);

                #region Delete Image

                if (book.ImageName != null)
                {
                    if (book.ImageName != "Defualt.jpg")
                    {
                        string imgDeletePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/Book/image",
                            book.ImageName);
                        if (File.Exists(imgDeletePath))
                        {
                            File.Delete(imgDeletePath);
                        }

                        string thumbDeletePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/Book/thumb",
                            book.ImageName);
                        if (File.Exists(thumbDeletePath))
                        {
                            File.Delete(thumbDeletePath);
                        }
                    }
                }

                #endregion

                book.ImageName = "Defualt.jpg";
                book.IsDelete = true;
                UpdateBook(book);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool UpdateBook(Book book)
        {
            return _productRepository.UpdateBook(book);
        }

        public Book GetBookById(int bookId)
        {
            return _productRepository.GetBookById(bookId);
        }

        public List<SelectListItem> GetGroupsForSelect()
        {
            return GetGroupsForAdmin()
                .Where(g => g.ParentId == null)
                .Select(g => new SelectListItem
                {
                    Value = g.GroupId.ToString(),
                    Text = g.GroupTitle
                }).ToList();
        }

        public List<SelectListItem> GetPublishersForSelect()
        {
            return GetPublishers()
                .Select(p => new SelectListItem
                {
                    Value = p.PublisherId.ToString(),
                    Text = p.PublisherName
                }).ToList();
        }

        public bool EditBook(Book book, IFormFile imgFile)
        {
            // update image
            if (imgFile != null)
            {
                string imgPath;
                string imgThumbPath;
                if (book.ImageName != "Defualt.jpg")
                {
                    // get old avatar path
                    imgPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/Book/image", book.ImageName);

                    // delete old avatar
                    if (File.Exists(imgPath))
                    {
                        File.Delete(imgPath);
                    }

                    // get old avatar thumb path
                    imgThumbPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/Book/thumb", book.ImageName);

                    // delete old thumb avatar
                    if (File.Exists(imgThumbPath))
                    {
                        File.Delete(imgThumbPath);
                    }
                }

                // generate new image path and name
                book.ImageName = CodeGenerator.GenerateUniqCode() + Path.GetExtension(imgFile.FileName);

                imgPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/Book/image", book.ImageName);

                // save file in file
                using (var stream = new FileStream(imgPath, FileMode.Create))
                {
                    imgFile.CopyTo(stream);
                }

                var imgResizer = new ImageConvertor();
                imgThumbPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/Book/thumb", book.ImageName);
                imgResizer.Image_resize(imgPath, imgThumbPath, 400);
            }

            return UpdateBook(book);
        }

        public FilterBookListViewModel GetBooksForIndex(FilterBookListViewModel filter, int? userId)
        {
            var result = _productRepository.GetBooksForAdmin().AsQueryable();

            // filter by price range
            #region price

            if (filter.MinPrice != null)
            {
                result = result.Where(r => r.Price >= filter.MinPrice);
            }
            if (filter.MaxPrice != null)
            {
                result = result.Where(r => r.Price <= filter.MaxPrice);
            }

            #endregion

            // filter by book Name
            #region book name

            if (!string.IsNullOrEmpty(filter.Search))
            {
                result = result.Where(r => EF.Functions.Like(r.Name, $"%{filter.Search}%"));
            }

            #endregion

            // filter by publisher name
            #region publisher

            if (!string.IsNullOrEmpty(filter.PublisherName))
            {
                result = result.Where(r => EF.Functions.Like(r.Publisher.PublisherName, $"%{filter.PublisherName}%"));
            }

            #endregion

            // filter by writer name
            #region writer

            if (!string.IsNullOrEmpty(filter.Writer))
            {
                result = result.Where(r => EF.Functions.Like(r.Writer, $"%{filter.Writer}%"));
            }

            #endregion

            // filter by age range
            #region age range

            if (filter.AgeRange != null)
            {
                result = result.Where(r => r.AgeRange == filter.AgeRange.Value);
            }

            #endregion

            // filter by selected groups
            #region selected group

            if (filter.SearchCategory != null && filter.SearchCategory.Any())
            {
                result = filter.SearchCategory
                    .Aggregate(result, (current, groupId)
                        => current.Where(g => g.GroupId == groupId ||
                                              g.SubGroupId == groupId ||
                                              g.SubGroup2Id == groupId));
            }

            #endregion

            // filter by selected publishers
            #region selected publishers

            if (filter.SelectedPublishers != null && filter.SelectedPublishers.Any())
            {
                result = filter.SelectedPublishers
                    .Aggregate(result, (current, publisherId)
                        => current.Where(p => p.PublisherId == publisherId));
            }

            #endregion

            // filter by exist books
            #region exist

            if (filter.Exist)
            {
                result = result.Where(b => b.Inventory != null && b.Inventory != 0);
            }

            #endregion

            //paging
            #region paging

            filter.Take = 16;
            var pager = Pager.Build(filter.PageNum, result.Count(), filter.Take, filter.PageCountAfterAndBefor);
            if (userId != null)
            {
                var books = result.Select(b => new BookListViewModel
                {
                    BookId = b.BookId,
                    ImageName = b.ImageName,
                    Name = b.Name,
                    Price = b.Price,
                    PublisherName = b.Publisher.PublisherName,
                    Writer = b.Writer,
                    BookInventory = b.Inventory,
                    BookRate = (int)(_productRepository.GetBookAverageScore(b.BookId) * 20),
                    IsLiked = _productRepository.IsUserLikedBook((int)userId, b.BookId)
                }).Paging(pager).ToList();
                return filter.SetPaging(pager).SetBooks(books);
            }
            var booksList = result.Select(b => new BookListViewModel
            {
                BookId = b.BookId,
                ImageName = b.ImageName,
                Name = b.Name,
                Price = b.Price,
                PublisherName = b.Publisher.PublisherName,
                Writer = b.Writer,
                BookInventory = b.Inventory,
                BookRate = (int)(_productRepository.GetBookAverageScore(b.BookId) * 20),
                IsLiked = false
            }).Paging(pager).ToList();
            return filter.SetPaging(pager).SetBooks(booksList);
            #endregion
        }

        public async Task<List<BookListViewModel>> GetLatestBooksInIndex(int take, int? userId)
        {
            if (userId != null)
            {
                var books = await _productRepository.GetLatestBook(take);
                var bookListViewModels = books.Select(b => new BookListViewModel
                {
                    BookId = b.BookId,
                    ImageName = b.ImageName,
                    PublisherName = b.Publisher.PublisherName,
                    Name = b.Name,
                    Price = b.Price,
                    Writer = b.Writer,
                    BookInventory = b.Inventory,
                    BookRate = (int)(_productRepository.GetBookAverageScore(b.BookId) * 20),
                    IsLiked = _productRepository.IsUserLikedBook((int)userId, b.BookId)
                }).ToList();
                return bookListViewModels;
            }

            var bookList = await _productRepository.GetLatestBook(take);
            var listViewModels = bookList.Select(b => new BookListViewModel
            {
                BookId = b.BookId,
                ImageName = b.ImageName,
                PublisherName = b.Publisher.PublisherName,
                Name = b.Name,
                Price = b.Price,
                Writer = b.Writer,
                BookInventory = b.Inventory,
                BookRate = (int)(_productRepository.GetBookAverageScore(b.BookId) * 20),
                IsLiked = false
            }).ToList();
            return listViewModels;
        }

        public Book GetBookForShowByBookId(int bookId)
        {
            return _productRepository.GetBookForShowByBookId(bookId);
        }

        public IEnumerable<BookListViewModel> PublisherBooks(int publisherId, Book book, int? userId)
        {
            var publisherBookList = _productRepository.PublisherBooks(publisherId).ToList();
            if (publisherBookList.FirstOrDefault(b => b.BookId == book.BookId) != null)
            {
                publisherBookList.Remove(book);
            }

            if (userId != null)
            {
                return publisherBookList.Select(b => new BookListViewModel
                {
                    BookId = b.BookId,
                    ImageName = b.ImageName,
                    PublisherName = b.Publisher.PublisherName,
                    Name = b.Name,
                    Price = b.Price,
                    Writer = b.Writer,
                    BookInventory = b.Inventory,
                    BookRate = (int)(_productRepository.GetBookAverageScore(b.BookId) * 20),
                    IsLiked = _productRepository.IsUserLikedBook((int)userId, b.BookId)
                });
            }
            return publisherBookList.Select(b => new BookListViewModel
            {
                BookId = b.BookId,
                ImageName = b.ImageName,
                PublisherName = b.Publisher.PublisherName,
                Name = b.Name,
                Price = b.Price,
                Writer = b.Writer,
                BookInventory = b.Inventory,
                BookRate = (int)(_productRepository.GetBookAverageScore(b.BookId) * 20),
                IsLiked = false
            });
        }

        public ChangeInventoryViewModel GetInventoryInfoByBookId(int bookId)
        {
            var book = GetBookById(bookId);
            return new ChangeInventoryViewModel
            {
                BookName = book.Name,
                CurrentInventory = book.Inventory,
                BookId = bookId
            };
        }

        public bool IncreaseInventory(ChangeInventoryViewModel inventory)
        {
            var book = GetBookById(inventory.BookId);

            // if book.inv == null -> book.inv = 0
            book.Inventory ??= 0;

            // sum
            var sum = book.Inventory + inventory.IncNumber;
            book.Inventory = sum;
            AddInventoryReport(inventory.BookId, Report.IncreaseChangeId, (int)inventory.IncNumber, inventory.BookName);

            return UpdateBook(book);
        }

        public string DecreaseInventory(ChangeInventoryViewModel inventory)
        {
            var book = GetBookById(inventory.BookId);

            // if book.inv == null - book.inv = 0
            book.Inventory ??= 0;

            // sum
            var mines = book.Inventory - inventory.DecNumber;
            if (mines < 0)
            {
                return "NegativeError";
            }

            book.Inventory = mines;
            AddInventoryReport(inventory.BookId, Report.DecreaseChangeId, (int)inventory.DecNumber, inventory.BookName);

            UpdateBook(book);
            return "Success";
        }

        public void AddInventoryReport(int bookId, int changeId, int changeNumber, string bookName)
        {
            _productRepository.AddInventoryReport(bookId, changeId, changeNumber, bookName);
        }

        public IEnumerable<InventoryReport> GetBookChangeInventoryReports(int bookId)
        {
            return _productRepository.GetBookInventoryReports(bookId);
        }

        public IEnumerable<AllInventoryReportsViewModel> GetAllInventoryReports()
        {
            return _productRepository.GetAllInventoryReports()
                .OrderByDescending(s => s.Date)
                .Select(r => new AllInventoryReportsViewModel
                {
                    ReportId = r.ReportId,
                    BookName = r.BookName,
                    ChangeNumber = r.ChangeNumber,
                    Date = r.Date,
                    ChangeId = r.ChangeId
                });
        }

        public IEnumerable<BookListViewModel> GetBooksByAgeRange(string userName, int? userId)
        {
            var userAge = GetAgeByUserName(userName);
            if (userAge == null) return null;

            var result = _productRepository.GetBooksForAdmin().AsQueryable();

            switch (userAge)
            {
                case <= 10:
                    result = result.Where(r => r.AgeRange == AgeRange.Kid);
                    break;
                case > 10 and <= 18:
                    result = result.Where(r => r.AgeRange == AgeRange.Teenager);
                    break;
                case > 18:
                    result = result.Where(r => r.AgeRange == AgeRange.Adult);
                    break;
            }

            if (userId != null)
            {
                return result.Select(r => new BookListViewModel
                {
                    BookId = r.BookId,
                    ImageName = r.ImageName,
                    PublisherName = r.Publisher.PublisherName,
                    Name = r.Name,
                    Price = r.Price,
                    Writer = r.Writer,
                    BookInventory = r.Inventory,
                    BookRate = (int)(_productRepository.GetBookAverageScore(r.BookId) * 20),
                    IsLiked = _productRepository.IsUserLikedBook((int)userId, r.BookId)
                }).Take(10);
            }
            return result.Select(r => new BookListViewModel
            {
                BookId = r.BookId,
                ImageName = r.ImageName,
                PublisherName = r.Publisher.PublisherName,
                Name = r.Name,
                Price = r.Price,
                Writer = r.Writer,
                BookInventory = r.Inventory,
                BookRate = (int)(_productRepository.GetBookAverageScore(r.BookId) * 20),
                IsLiked = false
            }).Take(10);
        }

        public int? GetAgeByUserName(string userName)
        {
            return _productRepository.GetUserByUserName(userName)?.Age;
        }

        public bool AddBookToFavorite(FavoriteBook favoriteBook)
        {
            favoriteBook.IsLiked = true;
            _productRepository.AddBookToFavorite(favoriteBook);
            return true;
        }

        public FavoriteBook GetFavBookInfoFromBook(int userId, int bookId)
        {
            return new FavoriteBook
            {
                BookId = bookId,
                IsLiked = IsUserLikedBook(userId, bookId)
            };
        }

        public bool IsUserLikedBook(int userId, int bookId)
        {
            return _productRepository.IsUserLikedBook(userId, bookId);
        }

        public IEnumerable<BookListViewModel> GetFavBooksByBookIds(List<int> bookIds)
        {
            return _productRepository.GetFavBooksByBookIds(bookIds)
                .Select(b => new BookListViewModel
                {
                    BookId = b.BookId,
                    ImageName = b.ImageName,
                    Name = b.Name,
                    Price = b.Price,
                    BookInventory = b.Inventory,
                    Writer = b.Writer
                });
        }

        public int GetFavBookIdByBookIdAndUserId(int userId, int bookId)
        {
            return _productRepository.GetFavBookIdByBookIdAndUserId(userId, bookId);
        }

        public bool RemoveFromFav(int likeId)
        {
            var fav = GetFavById(likeId);
            fav.IsDelete = true;
            _productRepository.UpdateFavorite(fav);
            return true;
        }

        public FavoriteBook GetFavById(int likeId)
        {
            return _productRepository.GetFavById(likeId);
        }

        public AddScoreResult AddScore(AddBookScoreViewModel addScore)
        {
            try
            {
                if (addScore.QualityScore < 0 || addScore.ContentScore < 0 || addScore.QualityScore > 5 || addScore.ContentScore > 5)
                {
                    return AddScoreResult.OutRangeScoreValue;
                }
                var score = new BookScore
                {
                    UserId = addScore.UserId,
                    BookId = addScore.BookId,
                    UserIp = addScore.UserIp,
                    QualityScore = addScore.QualityScore,
                    ContentScore = addScore.ContentScore,
                    ScoreDate = DateTime.Now,
                    AverageScores = (float)(addScore.QualityScore + addScore.ContentScore) / 2,
                    IsScored = true
                };

                return _productRepository.AddScore(score) ? AddScoreResult.Success : AddScoreResult.Failed;
            }
            catch
            {
                return AddScoreResult.Error;
            }
        }

        public bool IsUserBoughtBook(int userId, int bookId)
        {
            return _productRepository.IsUserBoughtBook(userId, bookId);
        }

        public bool ScoreSentByUser(int userId, int bookId)
        {
            return _productRepository.ScoreSentByUser(userId, bookId);
        }

        public int AllBookSentScoresCount(int bookId)
        {
            return _productRepository.AllBookSentScoresCount(bookId);
        }

        public float GetBookAverageScore(int bookId)
        {
            return _productRepository.GetBookAverageScore(bookId);
        }

        public int SatisfiedBookBuyersPercent(int bookId)
        {
            return _productRepository.SatisfiedBookBuyersPercent(bookId);
        }

        public IEnumerable<BookListViewModel> GetUserBooksForShowInUserPanel(int userId)
        {
            return _productRepository.GetUserBooks(userId).Select(b => new BookListViewModel
            {
                BookId = b.BookId,
                ImageName = b.ImageName,
                Name = b.Name,
                Writer = b.Writer
            });
        }

        public IEnumerable<Book> GetBooksByName(string bookName)
        {
            return _productRepository.GetBooksByName(bookName);
        }

        public IEnumerable<BookListViewModel> GetBestSellingBooks()
        {
            return _productRepository.GetBestSellingBooks()
                .Select(b => new BookListViewModel
                {
                    BookId = b.BookId,
                    BookInventory = b.Inventory,
                    ImageName = b.ImageName,
                    Name = b.Name,
                    PublisherName = b.Publisher.PublisherName,
                    Price = b.Price,
                    Writer = b.Writer,
                    BookRate = (int)(_productRepository.GetBookAverageScore(b.BookId) * 20)
                });
        }

        public IEnumerable<string> GetBookNamesForAutoCompleteSearch(string search)
        {
            return _productRepository.GetBookNamesForAutoCompleteSearch(search);
        }

        public IEnumerable<PublisherInAdminViewModel> GetPublishersForAdmin()
        {
            return _productRepository.GetPublishers()
                .Select(p => new PublisherInAdminViewModel
                {
                    PublisherName = p.PublisherName,
                    PublisherId = p.PublisherId
                });
        }

        public int PublisherBooksCount(int publisherId)
        {
            return _productRepository.PublisherBooksCount(publisherId);
        }

        public IEnumerable<PublisherBooksViewModel> GetPublisherBooks(int publisherId)
        {
            return _productRepository.PublisherBooks(publisherId)
                .Select(b => new PublisherBooksViewModel
                {
                    BookId = b.BookId,
                    BookName = b.Name,
                    PublisherName = b.Publisher.PublisherName
                });
        }

        public bool DeletePublisher(int publisherId)
        {
            try
            {
                var publisher = _productRepository.GetPublisherById(publisherId);
                if (publisher == null)
                {
                    return false;
                }

                publisher.IsDelete = true;
                return _productRepository.UpdatePublisher(publisher);
            }
            catch
            {
                return false;
            }
        }

        public CreatePublisherResult AddPublisherFromAdmin(CreatePublisherViewModel publisher)
        {
            if (publisher == null)
            {
                return CreatePublisherResult.Error;
            }

            if (IsNotPublisherNameUnique(publisher.PublisherName))
            {
                return CreatePublisherResult.RepetitiousName;
            }

            var newPublisher = new Publisher
            {
                PublisherName = publisher.PublisherName
            };

            return _productRepository.AddPublisher(newPublisher) ? CreatePublisherResult.Success : CreatePublisherResult.Error;
        }

        public EditPublisherViewModel GetPublisherInfoForEdit(int publisherId)
        {
            var publisher = _productRepository.GetPublisherById(publisherId);

            return new EditPublisherViewModel
            {
                PublisherName = publisher.PublisherName,
                PublisherId = publisherId
            };
        }

        public EditPublisherResult EditPublisher(EditPublisherViewModel publisher)
        {
            try
            {
                if (IsNotPublisherNameUnique(publisher.PublisherName))
                {
                    return EditPublisherResult.RepetitiousName;
                }

                var newPublisher = new Publisher
                {
                    PublisherName = publisher.PublisherName,
                    PublisherId = publisher.PublisherId
                };

                return _productRepository.UpdatePublisher(newPublisher) ? EditPublisherResult.Success : EditPublisherResult.Error;
            }
            catch
            {
                return EditPublisherResult.Error;
            }
        }

        public IEnumerable<string> GetAllPublisherNames()
        {
            return _productRepository.GetAllPublisherNames();
        }

        public bool IsNotPublisherNameUnique(string publisherName)
        {
            return _productRepository.IsNotPublisherNameUnique(publisherName);
        }

        public List<BookUsersViewModel> GetBookUsers(int bookId)
        {
            var userIds = _productRepository.GetBookUserIds(bookId);
            if (!userIds.Any())
            {
                return new List<BookUsersViewModel>();
            }

            var users = new List<Domain.Models.User.User>();
            foreach (var id in userIds)
            {
                var user = _userRepository.GetUserById(id);
                if (user != null)
                {
                    users.Add(_userRepository.GetUserById(id));
                }
            }

            return users.Select(u => new BookUsersViewModel
            {
                UserName = u.UserName,
                UserId = u.UserId,
                UserImageName = u.AvatarName
            }).ToList();
        }

        public ShowBookInfoViewModel GetBookForShowInAdminBookInfo(int bookId)
        {
            var book = _productRepository.GetBookByIdWithIncludes(bookId);
            if (book == null)
            {
                return new ShowBookInfoViewModel();
            }

            var bookInfo = new ShowBookInfoViewModel
            {
                BookId = book.BookId,
                Name = book.Name,
                ImageName = book.ImageName,
                Abstract = book.Abstract,
                AgeRange = book.AgeRange,
                CoverType = book.CoverType,
                GroupName = book.Group.GroupTitle,
                Inventory = book.Inventory,
                PageDescription = book.PageDescription,
                PagesCount = book.PagesCount,
                Price = book.Price,
                PublisherName = book.Publisher.PublisherName,
                Writer = book.Writer,
                SubGroupName = book.SubGroup.GroupTitle
            };
            if (book.SubGroup2 != null)
            {
                bookInfo.SubGroup2Name = book.SubGroup2.GroupTitle;
            }

            return bookInfo;
        }

        public int GetLotteryWinner(int bookId)
        {
            var bookUserIds = _productRepository.GetBookUserIds(bookId).ToArray();
            if (!bookUserIds.Any())
            {
                return default;
            }

            var r = new Random();
            var result = bookUserIds[r.Next(bookUserIds.Length)];

            return result;
        }

        public async Task<DeleteProductCommentsResult> DeleteAllProductComments(int bookId)
        {
            try
            {
                var productCommentIds = await _productRepository.GetAllProductCommentIds(bookId);
                if (!productCommentIds.Any())
                {
                    return DeleteProductCommentsResult.NotHaveComment;
                }

                foreach (var id in productCommentIds)
                {
                    _commentRepository.DeleteComment(id);
                }

                return DeleteProductCommentsResult.Success;
            }
            catch
            {
                return DeleteProductCommentsResult.Error;
            }
        }

        public IEnumerable<ProductCommentForShowInAdminBookInfoViewModel> GetProductCommentsForAdminInBookInfo(int bookId)
        {
            return _commentRepository.GetProductComments(bookId)
                .Select(c => new ProductCommentForShowInAdminBookInfoViewModel
                {
                    CommentBody = c.Body,
                    CommentId = c.CommentId,
                    IsReadByAdmin = c.IsReadByAdmin,
                    SendDate = c.SendDate,
                    ProductName = c.Product.Name,
                    SenderName = c.UserName,
                    SenderIp = c.UserIp,
                    SenderEmail = c.Email,
                    SenderId = c.UserId
                });
        }

        public async Task<List<BookSelectedToFavoriteUsersViewModel>> GetAllBookSelectedToFavorites(int bookId)
        {
            var usersList = await _productRepository.GetAllBookSelectedToFavorites(bookId);
            return usersList.Select(q => new BookSelectedToFavoriteUsersViewModel
            {
                UserName = q.UserName,
                ImageName = q.AvatarName,
                UserId = q.UserId
            }).ToList();
        }

        public async Task<List<BookScoreViewModel>> GetAllBookScores(int bookId)
        {
            var scoresList = await _productRepository.GetAllBookScores(bookId);
            return scoresList.Select(s => new BookScoreViewModel
            {
                BookId = s.BookId,
                UserName = s.User.UserName,
                UserId = s.UserId,
                UserIp = s.UserIp,
                BookName = s.Book.Name,
                ContentScore = s.ContentScore,
                QualityScore = s.QualityScore,
                AverageScores = s.AverageScores,
                ScoreDate = s.ScoreDate,
                ScoreId = s.ScoreId
            }).ToList();
        }

        public async Task<List<BookOrderViewModel>> GetAllBookOrderDetails(int bookId)
        {
            var orderDetails = await _productRepository.GetAllBookOrderDetails(bookId);
            if (!orderDetails.Any())
            {
                return new List<BookOrderViewModel>();
            }

            return orderDetails.Select(d => new BookOrderViewModel
            {
                UserName = d.Order.User.UserName,
                OrderId = d.OrderId,
                Price = d.Price,
                CreateDate = d.Order.CreateDate,
                ProductId = d.ProductId,
                Count = d.Count,
                DetailId = d.DetailId,
                ProductName = d.Product.Name,
                IsFinally = d.Order.IsFinally,
                UserId = d.Order.UserId
            }).ToList();
        }

        public async Task<BestSellingsWithPagingViewModel> GetBestSellingBooksForAdmin(BestSellingsWithPagingViewModel model)
        {
            var query = await _productRepository.GetBestSellingBooksForAdmin();
            var result = query.Select(q => new BestSellingBookViewModel
            {
                BookId = q.BookId,
                ImageName = q.ImageName,
                BookName = q.Name,
                OrderCount = q.OrderDetails.Sum(d => d.Count)
            }).AsQueryable();

            //paging
            var pager = Pager.Build(model.PageNum, result.Count(), model.Take, model.PageCountAfterAndBefor);
            var books = result.Paging(pager).ToList();

            return model.SetPaging(pager).SetBooks(books);
        }

        public async Task<MostLikedBooksViewModelWithPaging> GetMostLikedBooksForAdmin(MostLikedBooksViewModelWithPaging model)
        {
            var query = await _productRepository.GetMostLikedBooksForAdmin();
            var result = query.Select(q => new MostLikedBooksViewModel
            {
                BookId = q.BookId,
                ImageName = q.ImageName,
                BookName = q.Name,
                LikesCount = q.FavoriteBook.Count(f => f.IsLiked)
            }).AsQueryable();

            //paging
            var pager = Pager.Build(model.PageNum, result.Count(), model.Take, model.PageCountAfterAndBefor);
            var books = result.Paging(pager).ToList();

            return model.SetPaging(pager).SetBooks(books);
        }

        public async Task<BestRatedBooksWithPaging> GetBestRatedBooksForAdmin(BestRatedBooksWithPaging model)
        {
            var query = await _productRepository.GetBestRatedBooksForAdmin();
            var result = query.Select(q => new BestRatedBooksViewModel
            {
                BookId = q.BookId,
                ImageName = q.ImageName,
                BookName = q.Name,
                AverageScore = q.BookScores.Sum(s => s.AverageScores),
                ContentScore = q.BookScores.Sum(s => s.ContentScore),
                QualityScore = q.BookScores.Sum(s => s.QualityScore)
            }).AsQueryable();

            //paging
            var pager = Pager.Build(model.PageNum, result.Count(), model.Take, model.PageCountAfterAndBefor);
            var books = result.Paging(pager).ToList();

            return model.SetPaging(pager).SetBooks(books);
        }

        public async Task<FilterAdvancedViewModel> FilterBooksForFilterAdvanced(FilterAdvancedViewModel filter)
        {
            var result = _productRepository.GetBooksForAdmin().AsQueryable();

            // filter by price range
            #region price

            if (filter.MinPrice != null)
            {
                result = result.Where(r => r.Price >= filter.MinPrice);
            }
            if (filter.MaxPrice != null)
            {
                result = result.Where(r => r.Price <= filter.MaxPrice);
            }

            #endregion

            // filter by page count range
            #region page

            if (filter.MinPageCount != null)
            {
                result = result.Where(r => r.PagesCount >= filter.MinPageCount);
            }
            if (filter.MaxPageCount != null)
            {
                result = result.Where(r => r.PagesCount <= filter.MaxPageCount);
            }

            #endregion

            // filter by book Name
            #region book name

            if (!string.IsNullOrEmpty(filter.BookName))
            {
                result = result.Where(r => r.Name.Contains(filter.BookName));
            }

            #endregion

            // filter by publisher id
            #region publisher

            if (filter.PublisherId != null)
            {
                result = result.Where(r => r.PublisherId == filter.PublisherId);
            }

            #endregion

            // filter by writer name
            #region writer

            if (!string.IsNullOrEmpty(filter.Writer))
            {
                result = result.Where(r => r.Writer.Contains(filter.Writer));
            }

            #endregion

            // filter by age range
            #region age

            if (filter.AgeRange != null)
            {
                result = result.Where(r => r.AgeRange == filter.AgeRange.Value);
            }

            #endregion

            // filter by cover type
            #region cover

            if (filter.CoverType != null)
            {
                result = result.Where(r => r.CoverType == filter.CoverType.Value);
            }

            #endregion

            // filter by is exist
            #region is exist

            if (filter.IsExist)
            {
                result = result.Where(r => r.Inventory != null && r.Inventory != 0);
            }

            #endregion

            filter.ResultCount = await result.CountAsync();

            //paging
            var pager = Pager.Build(filter.PageNum, filter.ResultCount, 12, filter.PageCountAfterAndBefor);
            var books = await result.Paging(pager).Select(b => new BookListViewModel
            {
                BookId = b.BookId,
                Name = b.Name,
                BookInventory = b.Inventory,
                ImageName = b.ImageName,
                PublisherName = b.Publisher.PublisherName,
                Price = b.Price,
                Writer = b.Writer
            }).ToListAsync();


            return filter.SetPaging(pager).SetBooks(books);
        }

        public void AddCompare(Compare compare)
        {
            _productRepository.AddCompare(compare);
        }

        public AddBookToFavoriteResult AddBookToFavoriteById(int bookId, int userId)
        {
            try
            {
                var favBook = _productRepository.GetFavBookByBookAndUserId(bookId, userId);
                if (favBook == null)
                {
                    var newFavBook = new FavoriteBook
                    {
                        BookId = bookId,
                        UserId = userId,
                        IsLiked = true
                    };

                    _productRepository.AddBookToFavorite(newFavBook);
                    return AddBookToFavoriteResult.SuccessLike;
                }

                if (IsUserLikedBook(userId, bookId))
                {
                    favBook.IsLiked = false;
                    favBook.IsDelete = true;
                    _productRepository.UpdateFavorite(favBook);
                    return AddBookToFavoriteResult.SuccessUnlike;
                }

                favBook.IsLiked = true;
                _productRepository.UpdateFavorite(favBook);
                return AddBookToFavoriteResult.SuccessLike;
            }
            catch
            {
                return AddBookToFavoriteResult.Error;
            }
        }
    }
}
