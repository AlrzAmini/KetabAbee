﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using KetabAbee.Application.DTOs.Book;
using KetabAbee.Application.Extensions;
using KetabAbee.Application.Interfaces.Order;
using KetabAbee.Application.Interfaces.Product;
using KetabAbee.Domain.Models.Products;
using Microsoft.AspNetCore.Authorization;

namespace KetabAbee.Web.Controllers
{
    public class BookController : Controller
    {
        #region construcor

        private readonly IProductService _productService;
        private readonly IOrderService _orderService;

        public BookController(IProductService productService, IOrderService orderService)
        {
            _productService = productService;
            _orderService = orderService;
        }

        #endregion

        #region Books & Filter

        [HttpGet("Books")]
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
                var reqUrl = Microsoft.AspNetCore.Http.Extensions.UriHelper.GetDisplayUrl(Request);
                ViewBag.reqUrl = reqUrl;
                ViewBag.FavBook = _productService.GetFavBookInfoFromBook(int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)), model.BookId);
                var userName = User.Identity.Name;
                ViewBag.AgeRangeBooks = _productService.GetBooksByAgeRange(userName).ToList();
                ViewBag.UserAge = _productService.GetAgeByUserName(userName);
            }

            return View(model);
        }

        #endregion

        #region Add Book to Favorite

        [Authorize]
        [HttpPost("AddToFavorite")]
        public IActionResult AddToFavorite(FavoriteBook favoriteBook)
        {
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

        #region remove book from favorite

        [HttpGet("RemoveF/{likeId}")]
        public IActionResult RemoveBookFromFavorite(int likeId)
        {
            if (_productService.RemoveFromFav(likeId))
            {
                TempData["SuccessMessage"] = "حذف از علاقه مندی ها با موفقیت انجام شد";
                return Redirect("/UserPanel/Dashboard");
            }
            TempData["ErrorMessage"] = "حذف از علاقه مندی ها با شکست مواجه شد";
            return Redirect("/UserPanel/Dashboard");
        }

        #endregion

        #region buy book

        [Authorize]
        [Route("AddToCart")]
        public IActionResult AddToCart(int productId)
        {
            var orderId = _orderService.AddOrder(User.GetUserId(), productId);
            switch (orderId)
            {
                case 0:
                    TempData["ErrorSwal"] = "به سبد خرید افزوده نشد";
                    break;
                case -1:
                    TempData["WarningSwal"] = "موجودی این کالا کافی نیست";
                    break;
            }
            if (orderId != 0 && orderId != -1)
            {
                TempData["SuccessSwal"] = "به سبد خرید افزوده شد";
            }

            return Redirect($"/Cart/{orderId}");
        }

        #endregion

    }
}
