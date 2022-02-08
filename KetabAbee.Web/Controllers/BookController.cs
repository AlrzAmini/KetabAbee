﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using KetabAbee.Application.DTOs.Book;
using KetabAbee.Application.Extensions;
using KetabAbee.Application.Interfaces.Comment;
using KetabAbee.Application.Interfaces.Order;
using KetabAbee.Application.Interfaces.Product;
using KetabAbee.Domain.Models.Comment.ProductComment;
using KetabAbee.Domain.Models.Products;
using Microsoft.AspNetCore.Authorization;

namespace KetabAbee.Web.Controllers
{
    public class BookController : Controller
    {
        #region construcor

        private readonly IProductService _productService;
        private readonly IOrderService _orderService;
        private readonly ICommentService _commentService;

        public BookController(IProductService productService, IOrderService orderService, ICommentService commentService)
        {
            _productService = productService;
            _orderService = orderService;
            _commentService = commentService;
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
            ViewBag.CommentCount = _commentService.ProductCommentCount(bookId);
            if (User.Identity.IsAuthenticated)
            {
                ViewBag.FavBook = _productService.GetFavBookInfoFromBook(User.GetUserId(), model.BookId);
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
            favoriteBook.UserId = User.GetUserId();

            if (_productService.AddBookToFavorite(favoriteBook))
            {
                TempData["SuccessSwal"] = "به لیست علاقه مندی ها اضافه شد";
                return Redirect($"/BookInfo/{favoriteBook.BookId}");
            }

            TempData["ErrorSwal"] = "به لیست علاقه مندی ها اضافه نشد";
            return Redirect($"/BookInfo/{favoriteBook.BookId}");
        }

        #endregion

        #region remove book from favorite

        [HttpGet("RemoveF/{likeId}")]
        public IActionResult RemoveBookFromFavorite(int likeId)
        {
            if (_productService.RemoveFromFav(likeId))
            {
                TempData["SuccessSwal"] = "حذف از علاقه مندی ها با موفقیت انجام شد";
                return Redirect("/UserPanel/Dashboard");
            }
            TempData["ErrorSwal"] = "حذف از علاقه مندی ها با شکست مواجه شد";
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

        #region add comment

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult AddComment(ProductComment comment)
        {
            if (User.Identity.IsAuthenticated)
            {
                comment.UserId = User.GetUserId();
                comment.UserName = User.Identity.Name;
                comment.Email = User.GetUserEmail();
            }
            comment.UserIp = HttpContext.GetUserIp();
            if (_commentService.AddComment(comment))
            {
                return View("ShowComments", _commentService.GetProductCommentWithPaging(comment.ProductId));
            }
            TempData["ErrorSwal"] = "کامنت شما ثبت نشد";
            return Redirect($"/BookInfo/{comment.ProductId}");
        }

        #endregion

        #region comments

        [Route("Book/ShowComments/{bookId}")]
        public IActionResult ShowComments(int bookId, int pageNum = 1)
        {
            return View(_commentService.GetProductCommentWithPaging(bookId, pageNum));
        }

        #endregion

        #region add answer

        [Authorize]
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult AddAnswer(ProductCommentAnswer answer, int productId)
        {
            answer.Email = User.GetUserEmail();
            answer.UserId = User.GetUserId();
            answer.UserIp = HttpContext.GetUserIp();
            answer.UserName = User.Identity.Name;

            if (_commentService.AddAnswer(answer))
            {
                return View("ShowComments", _commentService.GetProductCommentWithPaging(productId));
            }
            TempData["ErrorSwal"] = "پاسخ شما ثبت نشد";
            return Redirect($"/BookInfo/{productId}");
        }

        #endregion

        #region add score

        [Authorize]
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult AddScore(int bookId, int qualityScore, int contentScore)
        {
            var userId = User.GetUserId();

            if (!_productService.IsUserBoughtBook(userId, bookId)) return BadRequest();

            if (_productService.AddScore(userId, bookId, HttpContext.GetUserIp(), qualityScore, contentScore))
            {
                TempData["SuccessSwal"] = "امتیاز شما برای این محصول ثبت شد";
                return Redirect($"/BookInfo/{bookId}");
            }
            TempData["ErrorSwal"] = "امتیاز شما ثبت نشد";
            return Redirect($"/BookInfo/{bookId}");
        }

        #endregion

        #region remove comment

        [HttpGet("Book/RemoveComment/{commentId}")]
        public IActionResult RemoveComment(int commentId, int bookId)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (!_commentService.IsUserSendComment(User.GetUserId(), commentId)) return Forbid();
                if (_commentService.DeleteComment(commentId))
                {
                    TempData["SuccessSwal"] = "دیدگاه با موفقیت حذف شد";
                    return Redirect($"/BookInfo/{bookId}");
                }
                TempData["ErrorSwal"] = "مشکلی در حذف دیدگاه رخ داد";
                return Redirect($"/BookInfo/{bookId}");

            }

            if (!_commentService.IsUserSendComment(HttpContext.GetUserIp(), commentId)) return Forbid();
            if (_commentService.DeleteComment(commentId))
            {
                TempData["SuccessSwal"] = "دیدگاه با موفقیت حذف شد";
                return Redirect($"/BookInfo/{bookId}");
            }
            TempData["ErrorSwal"] = "مشکلی در حذف دیدگاه رخ داد";
            return Redirect($"/BookInfo/{bookId}");

        }

        #endregion

        #region remove answer

        [Authorize]
        [HttpGet("Book/RemoveAnswer/{answerId}")]
        public IActionResult RemoveAnswer(int answerId, int bookId)
        {
            if (!_commentService.IsUserSendAnswer(User.GetUserId(), answerId)) return Forbid();
            if (_commentService.DeleteAnswer(answerId))
            {
                TempData["SuccessSwal"] = "پاسخ با موفقیت حذف شد";
                return Redirect($"/BookInfo/{bookId}");
            }
            TempData["ErrorSwal"] = "مشکلی در حذف دیدگاه رخ داد";
            return Redirect($"/BookInfo/{bookId}");
        }

        #endregion
    }
}
