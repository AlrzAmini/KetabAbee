using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using KetabAbee.Application.Convertors;
using KetabAbee.Application.DTOs;
using KetabAbee.Application.Extensions;
using KetabAbee.Application.Interfaces.Comment;
using KetabAbee.Application.Interfaces.Order;
using KetabAbee.Application.Interfaces.Product;
using KetabAbee.Application.Interfaces.User;
using KetabAbee.Application.Security;
using KetabAbee.Domain.Models.Task;

namespace KetabAbee.Web.Areas.UserPanel.Controllers
{
    public class UserPanelController : UserPanelBaseController
    {
        #region constructor

        private readonly IUserService _userService;
        private readonly IProductService _productService;
        private readonly IOrderService _orderService;
        private readonly ICommentService _commentService;

        public UserPanelController(IUserService userService, IProductService productService, IOrderService orderService, ICommentService commentService)
        {
            _userService = userService;
            _productService = productService;
            _orderService = orderService;
            _commentService = commentService;
        }

        #endregion

        #region Dashborad

        [HttpGet("UserPanel/Dashboard")]
        public async Task<IActionResult> Dashboard()
        {
            var userInfo = _userService.GetInfoByUserEmail(User.GetUserEmail());

            var userBookIds = await _userService.GetUserFavBookIds(User.GetUserId());

            ViewBag.FavBooks = _productService.GetFavBooksByBookIds(userBookIds).ToList();

            ViewBag.Orders = _orderService.GetUserFinalOrders(User.GetUserId()).ToList();

            return View(userInfo);
        }

        #endregion

        #region Edit Profile

        [HttpGet("UserPanel/EditProfile")]
        public IActionResult EditProfile()
        {
            var infoForEdit = _userService.GetInfoForEditInUserPanel(User.Identity.Name);
            return View(infoForEdit);
        }

        [HttpPost("UserPanel/EditProfile") , ValidateAntiForgeryToken]
        public IActionResult EditProfile(UserPanelEditInfoViewModel edit)
        {
            if (!ModelState.IsValid)
            {
                return View(edit);
            }

            edit.BirthDay = edit.BirthDay?.ToString(CultureInfo.InvariantCulture).StringShamsiToMiladi();

            if (_userService.EditUserProfile(edit))
            {
                TempData["SuccessSwal"] = "پروفایل شما با موفقیت بروزرسانی شد";

                return RedirectToAction("Dashboard");
            }

            TempData["ErrorSwal"] = "خطایی در هنگام ویرایش اطلاعات شما رخ داد";

            return RedirectToAction("Dashboard");

        }

        #endregion

        #region Change Password

        [HttpGet("UserPanel/ChangePassword")]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost("UserPanel/ChangePassword") , ValidateAntiForgeryToken]
        public IActionResult ChangePassword(ChangePasswordViewModel change)
        {
            if (!ModelState.IsValid)
            {
                return View(change);
            }

            string userName = User.Identity.Name;

            if (!_userService.IsOldPasswordCorrect(userName,change.OldPassword))
            {
                TempData["ErrorSwal"] = "کلمه عبور فعلی شما همخوانی ندارد ";
                return View(change);
            }

            // check password strength
            if (PasswordStrengthChecker.CheckStrength(change.Password) == PasswordScore.VeryWeak)
            {
                TempData["WarningSwal"] = "کلمه عبور جدید بسیار ضعیف است";
                TempData["ErrorMessage"] = "کلمه عبور می بایست بیش از 6 کاراکتر داشته باشد";
                return View(change);
            }

            if (!_userService.ChangePasswordInUserPanel(userName, change.Password)) return View(change);
            TempData["SuccessSwal"] = "کلمه عبور شما با موفقیت بروزرسانی شد ";
            return RedirectToAction("Dashboard");
        }

        #region check strength

        [HttpGet("UserPanel/Get/Password-Strength-Json")]
        public IActionResult GetPasswordStrengthJson(string password)
        {
            var data = password.CheckPasswordStrength();
            return new JsonResult(data);
        }

        #endregion

        #endregion

        #region Favorites

        [HttpGet("UserPanel/Favorites")]
        public async Task<IActionResult> FavoriteBooks()
        {
            var userBookIds = await _userService.GetUserFavBookIds(User.GetUserId()); 
            
            return View(_productService.GetFavBooksByBookIds(userBookIds).ToList());
        }

        #endregion

        #region User Books

        [HttpGet("UserPanel/Books")]
        public IActionResult UserBooks()
        {
            return View(_productService.GetUserBooksForShowInUserPanel(User.GetUserId()).ToList());
        }

        #endregion

        #region user comment

        [HttpGet("UserPanel/Comments")]
        public IActionResult UserComments()
        {
            return View(_commentService.GetUserCommentsInUserPanel(User.GetUserId()).ToList());
        }

        #endregion

    }
}
