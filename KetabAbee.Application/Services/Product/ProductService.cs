using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KetabAbee.Application.Convertors;
using KetabAbee.Application.DTOs.Admin.Products.Book;
using KetabAbee.Application.DTOs.Book;
using KetabAbee.Application.DTOs.Paging;
using KetabAbee.Application.Generators;
using KetabAbee.Application.Interfaces.Product;
using KetabAbee.Application.Security;
using KetabAbee.Domain.Interfaces;
using KetabAbee.Domain.Models.Products;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace KetabAbee.Application.Services.Product
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public bool AddGroup(ProductGroup group)
        {
            return _productRepository.AddGroup(group);
        }

        public IEnumerable<ProductGroup> GetGroups()
        {
            return _productRepository.GetGroups();
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

        public IEnumerable<Publisher> GetPublishers()
        {
            return _productRepository.GetPublishers();
        }

        public List<SelectListItem> GetGroupsForAddBook()
        {
            return _productRepository.GetGroups().Where(g => g.ParentId == null)
                .Select(g => new SelectListItem
                {
                    Value = g.GroupId.ToString(),
                    Text = g.GroupTitle
                }).ToList();
        }

        public List<SelectListItem> GetSubGroupsForAddBook(int groupId)
        {
            return _productRepository.GetGroups().Where(g => g.ParentId == groupId)
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

            // filter by exist status 
            #region exist status

            if (filter.ExistStatus != null)
            {
                result = result.Where(r => r.ExistStatus == filter.ExistStatus.Value);
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

        public FilterBookListViewModel GetBooksForIndex(FilterBookListViewModel filter)
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

            // filter by selected groups
            #region selected group

            if (filter.SelectedGroups != null && filter.SelectedGroups.Any())
            {
                result = filter.SelectedGroups
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

            // filter by exist status 
            #region exist status

            if (filter.ExistStatus != null)
            {
                result = result.Where(r => r.ExistStatus == filter.ExistStatus.Value);
            }

            #endregion

            //paging
            filter.Take = 16;
            var pager = Pager.Build(filter.PageNum, result.Count(), filter.Take, filter.PageCountAfterAndBefor);
            var books = result.Select(b => new BookListViewModel
            {
                BookId = b.BookId,
                ImageName = b.ImageName,
                Name = b.Name,
                Price = b.Price,
                PublisherName = b.Publisher.PublisherName,
                Writer = b.Writer
            }).Paging(pager).ToList();


            return filter.SetPaging(pager).SetBooks(books);
        }

        public IEnumerable<BookListViewModel> GetLatestBooksInIndex(int take)
        {
            return _productRepository.GetLatestBook(take)
                .Select(b => new BookListViewModel
                {
                    BookId = b.BookId,
                    ImageName = b.ImageName,
                    PublisherName = b.Publisher.PublisherName,
                    Name = b.Name,
                    Price = b.Price,
                    Writer = b.Writer
                });

        }

        public Book GetBookForShowByBookId(int bookId)
        {
            return _productRepository.GetBookForShowByBookId(bookId);
        }

        public IEnumerable<BookListViewModel> PublisherBooks(int publisherId)
        {
            return _productRepository.PublisherBooks(publisherId)
                .Select(b => new BookListViewModel
                {
                    BookId = b.BookId,
                    ImageName = b.ImageName,
                    PublisherName = b.Publisher.PublisherName,
                    Name = b.Name,
                    Price = b.Price,
                    Writer = b.Writer
                });
        }
    }
}
