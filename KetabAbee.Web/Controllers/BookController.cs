using System;
using System.Collections.Generic;
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
using System.Threading.Tasks;
using KetabAbee.Application.DTOs.Comment;
using GoogleReCaptcha.V3.Interface;
using KetabAbee.Application.Const;
using KetabAbee.Application.DTOs.Admin.Exam;
using KetabAbee.Application.DTOs.Compare;
using KetabAbee.Application.Generators;
using KetabAbee.Application.Interfaces.Exam;
using KetabAbee.Application.Interfaces.Permission;
using KetabAbee.Domain.Models.Products.Exam;
using Microsoft.AspNetCore.Http;

namespace KetabAbee.Web.Controllers
{
    public class BookController : Controller
    {
        #region construcor

        private readonly IProductService _productService;
        private readonly IOrderService _orderService;
        private readonly ICommentService _commentService;
        private readonly IPermissionService _permissionService;
        private readonly IExamService _examService;

        public BookController(IProductService productService, IOrderService orderService, ICommentService commentService, IPermissionService permissionService, IExamService examService)
        {
            _productService = productService;
            _orderService = orderService;
            _commentService = commentService;
            _permissionService = permissionService;
            _examService = examService;
        }

        #endregion

        #region Books & Filter

        [HttpGet("Books")]
        public async Task<IActionResult> Index(FilterBookListViewModel filter)
        {
            ViewBag.Groups = await _productService.GetGroups();
            ViewBag.Publishers = _productService.GetPublishers().ToList();

            return View(User.Identity.IsAuthenticated ? _productService.GetBooksForIndex(filter, User.GetUserId()) : _productService.GetBooksForIndex(filter, null));
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

            var isAuth = User.Identity.IsAuthenticated;

            #region satisfied and avg score

            ViewData["SatisfiedUsersPercent"] = _productService.SatisfiedBookBuyersPercent(bookId);
            ViewData["BookScoresCount"] = _productService.AllBookSentScoresCount(bookId);

            #endregion

            #region publisher books

            if (isAuth)
            {
                ViewData["PublisherBooks"] = _productService.PublisherBooks(model.PublisherId, model, User.GetUserId()).ToList();
            }
            else
            {
                ViewData["PublisherBooks"] = _productService.PublisherBooks(model.PublisherId, model, null).ToList();
            }

            #endregion

            #region exam results

            if (!User.Identity.IsAuthenticated)
            {
                ViewBag.IsCurrentUserHaveResult = _examService.IsUserIpHaveAnyExam(HttpContext.GetUserIp());
            }

            #endregion

            if (!isAuth) return View(model);

            var userId = User.GetUserId();

            #region user bought book

            ViewData["IsUserBoughtBook"] = User.Identity.IsAuthenticated && _productService.IsUserBoughtBook(userId, bookId);

            #endregion

            #region scores

            ViewData["ScoreSentByUser"] = _productService.ScoreSentByUser(userId, bookId);

            #endregion

            #region favorite books

            ViewBag.FavBook = _productService.GetFavBookInfoFromBook(userId, bookId);

            #endregion

            #region age range books

            var userName = User.Identity.Name;

            var ageBooks = _productService.GetBooksByAgeRange(userName, userId);
            if (ageBooks == null)
            {
                ViewData["AgeRangeBooks"] = null;
            }
            else
            {
                ViewData["AgeRangeBooks"] = ageBooks.ToList();
            }

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

        [Authorize]
        [HttpGet]
        public IActionResult AddBookToFavoriteFromBookBox(int bookId, string backUrl)
        {
            var res = _productService.AddBookToFavoriteById(bookId, User.GetUserId());
            switch (res)
            {
                case AddBookToFavoriteResult.SuccessLike:
                    TempData["SuccessSwal"] = "به لیست علاقه مندی ها اضافه شد";
                    return Redirect(backUrl);
                case AddBookToFavoriteResult.SuccessUnlike:
                    TempData["WarningSwal"] = "از لیست علاقه مندی ها حذف شد";
                    return Redirect(backUrl);
                case AddBookToFavoriteResult.Error:
                    TempData["ErrorSwal"] = "به لیست علاقه مندی ها اضافه نشد";
                    return Redirect(backUrl);
                default:
                    TempData["ErrorSwal"] = "به لیست علاقه مندی ها اضافه نشد";
                    return Redirect(backUrl);
            }
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
        public async Task<IActionResult> AddComment(CreateCommentViewModel comment)
        {
            if (User.Identity.IsAuthenticated)
            {
                comment.UserId = User.GetUserId();
                comment.UserName = User.Identity.Name;
                comment.Email = User.GetUserEmail();
            }
            comment.UserIp = HttpContext.GetUserIp();
            if (await _commentService.AddComment(comment))
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

        [HttpGet("load-modal-answer")]
        public IActionResult LoadAddAnswerModal(int commentId)
        {
            var comment = _commentService.GetCommentById(commentId);
            var answerModalModel = new CreateCommentAnswerViewModel
            {
                BookId = comment.ProductId,
                CommentId = comment.CommentId
            };
            return PartialView("_AddAnswerModal",answerModalModel);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> AddAnswer(CreateCommentAnswerViewModel answer)
        {
            if (User.Identity.IsAuthenticated)
            {
                // todo : if body was empty --> check in service
                answer.Email = User.GetUserEmail();
                answer.UserId = User.GetUserId();
                answer.UserIp = HttpContext.GetUserIp();
                answer.UserName = User.GetUserName();

                var res = await _commentService.AddAnswer(answer);
                switch (res)
                {
                    case CreateCommentAnswerResult.Success:
                        return View("ShowComments", _commentService.GetProductCommentWithPaging(answer.BookId));
                    case CreateCommentAnswerResult.EmptyBody:
                        return View("ShowComments", _commentService.GetProductCommentWithPaging(answer.BookId));
                    case CreateCommentAnswerResult.Error:
                        return View("ShowComments", _commentService.GetProductCommentWithPaging(answer.BookId));
                    default:
                        TempData["ErrorSwal"] = "پاسخ شما ثبت نشد";
                        return RedirectToAction("BookInfo", new { bookId = answer.BookId });
                }
            }

            TempData["InfoSwal"] = "برای ثبت پاسخ می بایست وارد حساب کاربری خود شوید";
            return RedirectToAction("BookInfo", new { bookId = answer.BookId });
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
                if (!_commentService.IsUserSendComment(User.GetUserId(), commentId) && !_permissionService.CheckPermission(PerIds.AdminPanel, User.GetUserId())) return BadRequest();
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
            // todo : admin can remove comments
            if (!_commentService.IsUserSendAnswer(User.GetUserId(), answerId) && !_permissionService.CheckPermission(PerIds.AdminPanel, User.GetUserId())) return BadRequest();
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

        #region exam

        [HttpGet("Exam-Guide/Book/{bookId}/{bookName}")]
        public async Task<IActionResult> ExamGuide(int bookId, string bookName)
        {
            if (!await _examService.IsUserCanDoExam(HttpContext.GetUserIp(), bookId))
            {
                TempData["WarningSwal"] = "نمیتوانید بیش از دوبار در آزمون هر کتاب شرکت کنید";
                return RedirectToAction("BookInfo", new { bookId, bookName });
            }

            var exam = await _examService.GetBookExamGuideInfo(bookId);
            if (exam != null)
            {
                return View(exam);
            }

            TempData["WarningSwal"] = "آزمونی برای این کتاب تعریف نشده است";
            return RedirectToAction("BookInfo", new { bookId, bookName });
        }

        [HttpGet("Live-Exam/{examId}")]
        public async Task<IActionResult> LiveExam(int examId)
        {
            var liveExam = await _examService.GetLiveExamInfo(examId);
            if (liveExam == null)
            {
                return NotFound();
            }

            if (await _examService.IsUserCanDoExam(HttpContext.GetUserIp(), liveExam.Exam.BookId)) return View(liveExam);

            TempData["WarningSwal"] = "اجازه دسترسی به این آزمون را ندارید";
            return RedirectToAction("BookInfo",
                new { bookId = liveExam.Exam.BookId, bookName = liveExam.Exam.Book.Name });
        }

        [HttpPost("Live-Exam/{examId}"), ValidateAntiForgeryToken]
        public async Task<IActionResult> LiveExam(IFormCollection iFormCollection, int examId)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("LiveExam", new { examId });
            }

            try
            {
                var correctsCount = 0;
                var wrongsCount = 0;
                string[] questionIds = iFormCollection["questionId"];
                foreach (var questionId in questionIds)
                {
                    var correctAnswerId = await _examService.GetQuestionCorrectAnswerId(int.Parse(questionId));
                    if (correctAnswerId == int.Parse(iFormCollection["question_" + questionId]))
                    {
                        correctsCount++;
                    }
                    else
                    {
                        wrongsCount++;
                    }
                }
                var examResult = new ExamResult
                {
                    UserIp = HttpContext.GetUserIp(),
                    ExamId = examId,
                    CorrectAnswersCount = correctsCount,
                    WrongAnswersCount = wrongsCount,
                    AllQuestionsCount = await _examService.GetExamQuestionsCount(examId)
                };

                var allAnswerCount = correctsCount + wrongsCount;

                if (examResult.AllQuestionsCount != allAnswerCount)
                {
                    TempData["WarningSwal"] = "برای ثبت آزمون میبایست تمام سوالات را پاسخ دهید";
                    return RedirectToAction("LiveExam", new { examId });
                }

                if (User.Identity.IsAuthenticated)
                {
                    examResult.UserId = User.GetUserId();
                }

                examResult.Score = await _examService.GetExamResultScore(examResult.AllQuestionsCount, correctsCount);
                var examResultId = await _examService.CreateExamResult(examResult);

                return RedirectToAction("ShowExamResult", new { examResultId });
            }
            catch
            {
                TempData["ErrorSwal"] = "زمان شما به اتمام رسید";
                var eRes = new ExamResult
                {
                    AllQuestionsCount = 0,
                    CorrectAnswersCount = 0,
                    ExamId = examId,
                    WrongAnswersCount = 0,
                    ResultStatus = ExamResultStatus.Failed,
                    Score = 0,
                    UserIp = HttpContext.GetUserIp()
                };
                if (User.Identity.IsAuthenticated)
                {
                    eRes.UserId = User.GetUserId();
                }

                await _examService.CreateExamResult(eRes);

                return RedirectToAction("Index", "Home");
            }
        }

        [HttpGet("Exam-Result")]
        public async Task<IActionResult> ShowExamResult(int examResultId)
        {
            var examResult = await _examService.GetExamResultById(examResultId);
            if (examResult.UserIp != HttpContext.GetUserIp())
            {
                return Forbid();
            }

            if (!User.Identity.IsAuthenticated) return View("ExamResult", examResult);

            if (examResult.UserId != null && examResult.UserId != User.GetUserId())
            {
                return Forbid();
            }

            return View("ExamResult", examResult);
        }

        [HttpGet("User-Exam-Results")]
        public async Task<IActionResult> ShowExamResults()
        {
            if (!User.Identity.IsAuthenticated)
            {
                var results = await _examService.GetUserExamResultsByIp(HttpContext.GetUserIp());
                if (results != null && results.Any())
                {
                    return View(results);
                }

                TempData["WarningMessage"] = "شما آزمونی انجام نداده اید";
                return View();
            }

            var resultList = await _examService.GetUserExamResultsById(User.GetUserId());
            if (resultList != null && resultList.Any())
            {
                return View(resultList);
            }

            TempData["WarningMessage"] = "شما آزمونی انجام نداده اید";
            return View();
        }

        #endregion

        #region compare

        public async Task<IActionResult> AddBookForCompare(int bookId, string backUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                var res = await _productService.AddCompare(HttpContext.GetUserIp(), bookId, User.GetUserId());
                switch (res)
                {
                    case null:
                        TempData["ErrorSwal"] = "مشکلی در انجام عملیات رخ داد";
                        return Redirect(backUrl);
                    case "RangeError":
                        TempData["WarningSwal"] = "توجه کنید که شما به صورت همزمان قادر به مقایسه چهار محصول در کنار هم هستید";
                        return Redirect(backUrl);
                    case "Repetitious":
                        TempData["WarningSwal"] = "محصول انتخابی برای مقایسه نمی تواند تکراری باشد";
                        return Redirect(backUrl);
                }

                TempData["SuccessSwal"] = "برای مقایسه افزوده شد";
                return RedirectToAction("ShowCompare", new { compareId = res });
            }

            var result = await _productService.AddCompare(HttpContext.GetUserIp(), bookId, null);
            switch (result)
            {
                case null:
                    TempData["ErrorSwal"] = "مشکلی در انجام عملیات رخ داد";
                    return Redirect(backUrl);
                case "RangeError":
                    TempData["WarningSwal"] = "توجه کنید که شما به صورت همزمان قادر به مقایسه چهار محصول در کنار هم هستید";
                    return Redirect(backUrl);
                case "Repetitious":
                    TempData["WarningSwal"] = "محصول انتخابی برای مقایسه نمی تواند تکراری باشد";
                    return Redirect(backUrl);
            }

            TempData["SuccessSwal"] = "برای مقایسه افزوده شد";
            return RedirectToAction("ShowCompare", new { compareId = result });

        }

        #region show compare

        [HttpGet("Comp/{compareId}")]
        public async Task<IActionResult> ShowCompare(string compareId)
        {
            ShowCompareToUserViewModel model;
            if (User.Identity.IsAuthenticated)
            {
                model = await _productService.GetCompareForShow(compareId, HttpContext.GetUserIp(), User.GetUserId());
            }
            else
            {
                model = await _productService.GetCompareForShow(compareId, HttpContext.GetUserIp(), null);
            }
            if (model == null)
            {
                return NotFound();
            }
            return View(model);
        }

        #endregion

        #region delete item from compare

        [HttpGet("{compareId}/Remove/{bookId}")]
        public async Task<IActionResult> DeleteItemFromCompare(int bookId, string compareId)
        {
            bool isLastCompare;
            if (User.Identity.IsAuthenticated)
            {
                isLastCompare = await _productService.IsCompareLastRecord(compareId, HttpContext.GetUserIp(), User.GetUserId());
            }
            else
            {
                isLastCompare = await _productService.IsCompareLastRecord(compareId, HttpContext.GetUserIp(), null);
            }

            if (!isLastCompare)
            {
                TempData["ErrorSwal"] = "نمیشه حذفش کرد :)";
                return RedirectToAction("ShowCompare", new { compareId });
            }

            if (await _productService.RemoveItemFromCompare(bookId, compareId))
            {
                TempData["SuccessSwal"] = "از مقایسه حذف شد";
                return RedirectToAction("ShowCompare", new { compareId });
            }
            TempData["ErrorSwal"] = "آیتم مورد نظر یافت نشد";
            return RedirectToAction("ShowCompare", new { compareId });
        }

        #endregion

        #region current user Compares

        [HttpGet("Compares")]
        public async Task<IActionResult> AllUserCompares()
        {
            List<CompareInListViewModel> compares;
            if (User.Identity.IsAuthenticated)
            {
                compares = await _productService.GetAllUserIdCompares(User.GetUserId());
            }
            else
            {
                compares = await _productService.GetAllUserIpCompares(HttpContext.GetUserIp());
            }
            return View(compares);
        }

        #endregion

        #endregion
    }
}
