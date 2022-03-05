using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KetabAbee.Application.Const;
using KetabAbee.Application.DTOs.Admin.Products.Options;
using KetabAbee.Application.Interfaces.Product;
using KetabAbee.Application.Interfaces.User;
using KetabAbee.Application.Security;

namespace KetabAbee.Web.Areas.AdminPanel.Controllers.Products
{
    [PermissionChecker(PerIds.Admin‌Books)]
    [Route("Admin/Book/Option")]
    public class AdminBookOptionsController : AdminBaseController
    {
        #region constructor

        private readonly IProductService _productService;
        private readonly IUserService _userService;

        public AdminBookOptionsController(IProductService productService, IUserService userService)
        {
            _productService = productService;
            _userService = userService;
        }

        #endregion

        #region buyers

        [HttpGet("{bookId}/Buyers")]
        public IActionResult BookBuyers(int bookId)
        {
            var users = _productService.GetBookUsers(bookId);
            if (users.Any()) return View(users);

            TempData["ErrorMessage"] = "این کتاب خریداری ندارد";
            return RedirectToAction("BookInfo", "AdminBook", new { bookId });
        }

        #endregion

        #region Lottery

        [HttpGet("{bookId}/Lottery-json")]
        public IActionResult Lottery(int bookId)
        {
            var winnerId = _productService.GetLotteryWinner(bookId);
            if (winnerId == 0)
            {
                const string res = "این کتاب خریداری جهت قرعه کشی ندارد";
                return new JsonResult(res);
            }
            var winnerName = _userService.GetUserNameByUserId(winnerId);
            var result = $"برنده قرعه کشی  {winnerName} با آی دی ' {winnerId} ' است";

            return new JsonResult(result);
        }

        #endregion

        #region book comments

        [HttpGet("{bookId}/Comments")]
        public IActionResult BookComments(int bookId)
        {
            var productComments = _productService.GetProductCommentsForAdminInBookInfo(bookId).ToList();
            if (productComments.Any()) return View(productComments);

            TempData["WarningMessage"] = "نظری برای این محصول ثبت نشده است";
            return RedirectToAction("BookInfo", "AdminBook", new { bookId });

        }

        #endregion

        #region delete all book comments

        public async Task<IActionResult> DeleteBookComments(int bookId)
        {
            var res = await _productService.DeleteAllProductComments(bookId);
            switch (res)
            {
                case DeleteProductCommentsResult.Success:
                    TempData["SuccessMessage"] = "تمام نظرات این محصول حذف شد";
                    return RedirectToAction("BookInfo", "AdminBook", new { bookId });
                case DeleteProductCommentsResult.NotHaveComment:
                    TempData["WarningMessage"] = "نظری برای این محصول ثبت نشده است";
                    return RedirectToAction("BookInfo", "AdminBook", new { bookId });
                case DeleteProductCommentsResult.Error:
                    TempData["ErrorMessage"] = "مشکلی در حذف نظرات رخ داد";
                    return RedirectToAction("BookInfo", "AdminBook", new { bookId });
                default:
                    return RedirectToAction("BookInfo", "AdminBook", new { bookId });
            }
        }

        #endregion

        #region favorite users

        [HttpGet("{bookId}/FavUsers")]
        public async Task<IActionResult> UsersLikeThisBook(int bookId)
        {
            var usersList = await _productService.GetAllBookSelectedToFavorites(bookId);
            ViewBag.UsersCount = usersList.Count;
            if (usersList.Any()) return View(usersList);

            TempData["WarningMessage"] = "لایکی برای این محصول ثبت نشده است";
            return RedirectToAction("BookInfo", "AdminBook", new { bookId });

        }

        #endregion

        #region book scores

        [HttpGet("{bookId}/Scores")]
        public async Task<IActionResult> BookScores(int bookId)
        {
            var scoreList = await _productService.GetAllBookScores(bookId);
            ViewBag.BookAverageScore = _productService.GetBookAverageScore(bookId);
            if (scoreList.Any()) return View(scoreList);

            TempData["WarningMessage"] = "امتیازی برای این محصول ثبت نشده است";
            return RedirectToAction("BookInfo", "AdminBook", new { bookId });

        }

        #endregion

        #region book orders

        [HttpGet("{bookId}/Orders")]
        public async Task<IActionResult> BookOrders(int bookId)
        {
            var orderList = await _productService.GetAllBookOrderDetails(bookId);
            ViewBag.OrdersCount = orderList.Count;
            if (orderList.Any()) return View(orderList);

            TempData["WarningMessage"] = "سفارشی برای این محصول ثبت نشده است";
            return RedirectToAction("BookInfo", "AdminBook", new { bookId });

        }

        #endregion
    }
}
