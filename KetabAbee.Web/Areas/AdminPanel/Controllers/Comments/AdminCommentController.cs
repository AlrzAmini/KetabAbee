using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KetabAbee.Application.Const;
using KetabAbee.Application.DTOs.Admin.Comment;
using KetabAbee.Application.Interfaces.Comment;
using KetabAbee.Application.Security;

namespace KetabAbee.Web.Areas.AdminPanel.Controllers.Comments
{
    [PermissionChecker(PerIds.AdminComments)]
    [Route("Admin/Comments")]
    public class AdminCommentController : AdminBaseController
    {
        #region constructor

        private readonly ICommentService _commentService;

        public AdminCommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        #endregion

        #region index

        [HttpGet]
        public IActionResult Index(FilterCommentsViewModel filter)
        {
            ViewData["ENGCommentsCount"] = _commentService.EnglishCommentsCount();
            return View(_commentService.FilterComments(filter));
        }

        #endregion

        #region delete comment

        [HttpGet("Delete/{commentId}")]
        public IActionResult DeleteComment(int commentId)
        {
            if (_commentService.DeleteComment(commentId))
            {
                TempData["SuccessMessage"] = "نظر با موفقیت حذف شد";
                return RedirectToAction("Index");
            }
            TempData["ErrorMessage"] = "نظر حذف نشد";
            return RedirectToAction("Index");
        }

        #endregion

        #region change is read status

        [HttpGet("Change/IsRead/{commentId}")]
        public IActionResult ChangeIsRead(int commentId)
        {
            var res = _commentService.ChangeCommentIsRead(commentId);
            switch (res)
            {
                case ChangeCommentIsReadResult.ChangedToRead:
                    TempData["SuccessMessage"] = "نظر خوانده شد";
                    return RedirectToAction("Index");
                case ChangeCommentIsReadResult.ChangedToIsNotRead:
                    TempData["SuccessMessage"] = "نظر به حالت خوانده نشده درآمد";
                    return RedirectToAction("Index");
                case ChangeCommentIsReadResult.NotFound:
                    TempData["ErrorMessage"] = "نظری یافت نشد";
                    return RedirectToAction("Index");
                case ChangeCommentIsReadResult.Error:
                    TempData["ErrorMessage"] = "خطایی در انجام عملیات رخ داد";
                    return RedirectToAction("Index");
                default:
                    TempData["ErrorMessage"] = "خطایی در انجام عملیات رخ داد";
                    return RedirectToAction("Index");
            }
        }

        #endregion

        #region delete english comments

        [HttpGet("Delete/AllEng")]
        public IActionResult DeleteAllEngComments()
        {
            var res = _commentService.DeleteEnglishComments();
            switch (res)
            {
                case DeleteEnglishCommentsResult.Success:
                    TempData["SuccessMessage"] = "نظرات انگلیسی با موفقیت حذف شدند";
                    return RedirectToAction("Index");
                case DeleteEnglishCommentsResult.NotFoundAnyEnglishComment:
                    TempData["InfoMessage"] = "کامنت انگلیسی یافت نشد";
                    return RedirectToAction("Index");
                case DeleteEnglishCommentsResult.Error:
                    TempData["ErrorMessage"] = "خطایی در انجام عملیات رخ داد";
                    return RedirectToAction("Index");
                default:
                    TempData["ErrorMessage"] = "خطایی در انجام عملیات رخ داد";
                    return RedirectToAction("Index");
            }
        }

        #endregion

        #region comment answers

        [HttpGet("C/{commentId}/Answers")]
        public IActionResult ShowCommentAnswers(int commentId)
        {
            var model = _commentService.GetCommentAnswers(commentId).ToList();
            if (model.Any()) return View(model);

            TempData["InfoMessage"] = "پاسخی برای این کامنت یافت نشد";
            return RedirectToAction("Index");
        }

        #endregion

        #region delete answer

        [HttpGet("Delete/A/{answerId}")]
        public IActionResult DeleteAnswer(int answerId, int commentId)
        {
            if (_commentService.DeleteAnswer(answerId))
            {
                TempData["SuccessMessage"] = "پاسخ با موفقیت حذف شد";
                return RedirectToAction("ShowCommentAnswers", new {commentId});
            }
            TempData["ErrorMessage"] = "پاسخ حذف نشد";
            return RedirectToAction("ShowCommentAnswers", new { commentId });
        }

        #endregion
    }
}
