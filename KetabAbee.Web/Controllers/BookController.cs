using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using KetabAbee.Application.DTOs.Book;
using KetabAbee.Application.Interfaces.Product;
using KetabAbee.Domain.Models.Products;

namespace KetabAbee.Web.Controllers
{
    public class BookController : Controller
    {
        #region construcor

        private readonly IProductService _productService;

        public BookController(IProductService productService)
        {
            _productService = productService;
        }

        #endregion

        #region Books & Filter

        [HttpGet("Book")]
        public IActionResult Index(FilterBookListViewModel filter)
        {
            ViewBag.Groups = _productService.GetGroups().ToList();
            ViewBag.Publishers = _productService.GetPublishers().ToList();

            return View(_productService.GetBooksForIndex(filter));
        }

        #endregion

        #region Book Info

        [HttpGet("BookInfo/{bookId}")]
        public IActionResult BookInfo(int bookId)
        {
            var model = _productService.GetBookForShowByBookId(bookId);
            ViewBag.PublisherBooks = _productService.PublisherBooks(model.PublisherId, model).ToList();
            if (User.Identity.IsAuthenticated)
            {
                ViewBag.FavBook = _productService.GetFavBookInfoFromBook(int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)), model.BookId);
                var userName = User.Identity.Name;
                ViewBag.AgeRangeBooks = _productService.GetBooksByAgeRange(userName).ToList();
                ViewBag.UserAge = _productService.GetAgeByUserName(userName);
            }

            return View(model);
        }

        #endregion

        #region Add Book to Favorite

        [HttpPost("AddToFavorite")]
        public IActionResult AddToFavorite(FavoriteBook favoriteBook)
        {
            if (!User.Identity.IsAuthenticated) return Redirect("/Login");

            favoriteBook.UserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            if (_productService.AddBookToFavorite(favoriteBook))
            {
                TempData["SuccessMessage"] = "به لیست علاقه مندی ها اضافه شد";
                return Redirect($"/BookInfo/{favoriteBook.BookId}");
            }

            TempData["ErrorMessage"] = "به لیست علاقه مندی ها اضافه نشد";
            return Redirect($"/BookInfo/{favoriteBook.BookId}");
        }

        #endregion

    }
}
