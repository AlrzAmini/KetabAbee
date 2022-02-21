using System;
using KetabAbee.Application.DTOs.Book;
using KetabAbee.Application.Extensions;
using KetabAbee.Application.Interfaces.Comment;
using KetabAbee.Application.Interfaces.Order;
using KetabAbee.Application.Interfaces.Product;
using KetabAbee.Domain.Models.Comment.ProductComment;
using KetabAbee.Domain.Models.Products;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using KetabAbee.Application.DTOs.Comment;
using GoogleReCaptcha.V3.Interface;

namespace KetabAbee.Web.Controllers
{
    public class BookController : Controller
    {
        #region construcor

        private readonly IProductService _productService;
        private readonly IOrderService _orderService;
        private readonly ICommentService _commentService;
        private readonly ICaptchaValidator _captchaValidator;

        public BookController(IProductService productService, IOrderService orderService, ICommentService commentService, ICaptchaValidator captchaValidator)
        {
            _productService = productService;
            _orderService = orderService;
            _commentService = commentService;
            _captchaValidator = captchaValidator;
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

        [HttpGet("BookInfo/{bookId}/{bookName}")]
        [HttpGet("BookInfo/{bookId}")]
        public IActionResult BookInfo(int bookId, string bookName)
        {
            var model = _productService.GetBookForShowByBookId(bookId);
            if (model == null)
            {
                return NotFound();
            }

            #region scores

            ViewData["SatisfiedUsersPercent"] = _productService.SatisfiedBookBuyersPercent(bookId);
            ViewData["BookAverageScore"] = _productService.GetBookAverageScore(bookId);

            #endregion

            #region publisher books

            ViewData["PublisherBooks"] = _productService.PublisherBooks(model.PublisherId, model).ToList();

            #endregion

            if (!User.Identity.IsAuthenticated) return View(model);

            var userId = User.GetUserId();

            #region user bought book

            ViewData["IsUserBoughtBook"] = User.Identity.IsAuthenticated && _productService.IsUserBoughtBook(userId, bookId);

            #endregion

            #region scores

            ViewData["BookScoresCount"] = _productService.AllBookSentScoresCount(bookId);
            ViewData["ScoreSentByUser"] = _productService.ScoreSentByUser(userId, bookId);

            #endregion

            #region favorite books

            ViewBag.FavBook = _productService.GetFavBookInfoFromBook(userId, bookId);

            #endregion

            #region age range books

            var userName = User.Identity.Name;
            ViewData["AgeRangeBooks"] = _productService.GetBooksByAgeRange(userName).ToList();
            ViewData["UserAge"] = _productService.GetAgeByUserName(userName);

            #endregion

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
        public IActionResult AddComment(CreateCommentViewModel comment)
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

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult AddAnswer(ProductCommentAnswer answer, int productId)
        {
            if (User.Identity.IsAuthenticated)
            {
                // todo : if body was empty --> check in service
                answer.Email = User.GetUserEmail();
                answer.UserId = User.GetUserId();
                answer.UserIp = HttpContext.GetUserIp();
                answer.UserName = User.Identity.Name;
                if (_commentService.AddAnswer(answer))
                {
                    return View("ShowComments", _commentService.GetProductCommentWithPaging(productId));
                }
                TempData["ErrorSwal"] = "پاسخ شما ثبت نشد";
                return RedirectToAction("BookInfo", new { bookId = productId });
            }

            TempData["InfoSwal"] = "برای ثبت پاسخ می بایست وارد حساب کاربری خود شوید";
            return RedirectToAction("BookInfo", new { bookId = productId });
        }

        #endregion

        #region add score

        [Authorize]
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult AddScore(AddBookScoreViewModel score)
        {
            score.UserId = User.GetUserId();
            score.UserIp = HttpContext.GetUserIp();

            var userId = score.UserId;
            var bookId = score.BookId;

            if (!_productService.IsUserBoughtBook(userId, bookId)) return BadRequest();

            var res = _productService.AddScore(score);
            switch (res)
            {
                case AddScoreResult.Success:
                    TempData["SuccessSwal"] = "امتیاز شما برای این محصول ثبت شد";
                    return RedirectToAction("BookInfo", new { bookId });
                case AddScoreResult.Failed:
                    TempData["ErrorSwal"] = "امتیاز شما ثبت نشد";
                    return RedirectToAction("BookInfo", new { bookId });
                case AddScoreResult.Error:
                    TempData["ErrorSwal"] = "مشکلی در ثبت امتیاز رخ داد";
                    return RedirectToAction("BookInfo", new { bookId });
                case AddScoreResult.OutRangeScoreValue:
                    return BadRequest();
                default:
                    TempData["ErrorSwal"] = "مشکلی در ثبت امتیاز رخ داد";
                    return RedirectToAction("BookInfo", new { bookId });
            }
        }

        #endregion

        #region remove comment

        [HttpGet("Book/RemoveComment/{commentId}")]
        public IActionResult RemoveComment(int commentId, int? bookId)
        {
            #region is auth

            if (User.Identity.IsAuthenticated)
            {
                if (!_commentService.IsUserSendComment(User.GetUserId(), commentId)) return Forbid();
                if (_commentService.DeleteComment(commentId))
                {
                    TempData["SuccessSwal"] = "دیدگاه با موفقیت حذف شد";
                    return Redirect(bookId == null ? "/UserPanel/Comments" : $"/BookInfo/{bookId}");
                }
                TempData["ErrorSwal"] = "مشکلی در حذف دیدگاه رخ داد";
                return Redirect(bookId == null ? "/UserPanel/Comments" : $"/BookInfo/{bookId}");
            }

            #endregion

            #region is not auth

            if (!_commentService.IsUserSendComment(HttpContext.GetUserIp(), commentId)) return Forbid();
            if (_commentService.DeleteComment(commentId))
            {
                TempData["SuccessSwal"] = "دیدگاه با موفقیت حذف شد";
                return Redirect($"/BookInfo/{bookId}");
            }
            TempData["ErrorSwal"] = "مشکلی در حذف دیدگاه رخ داد";
            return Redirect($"/BookInfo/{bookId}");

            #endregion
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

        #region get book comments count

        [HttpGet("Get/book-comments-count-json")]
        public IActionResult GetBookCommentsCount(int bookId)
        {
            var data = _commentService.ProductCommentCount(bookId);
            return new JsonResult(data);
        }

        #endregion
    }
}
