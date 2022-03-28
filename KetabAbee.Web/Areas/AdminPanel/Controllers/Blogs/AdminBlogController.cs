using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KetabAbee.Application.Const;
using KetabAbee.Application.DTOs.Admin.Blog;
using KetabAbee.Application.Interfaces.Blog;
using KetabAbee.Application.Interfaces.User;
using KetabAbee.Application.Security;

namespace KetabAbee.Web.Areas.AdminPanel.Controllers.Blogs
{
    [PermissionChecker(PerIds.AdminBlogs)]
    [Route("Admin/Blogs")]
    public class AdminBlogController : AdminBaseController
    {
        #region ctor

        private readonly IBlogService _blogService;
        private readonly IUserService _userService;

        public AdminBlogController(IBlogService blogService, IUserService userService)
        {
            _blogService = blogService;
            _userService = userService;
        }

        #endregion

        #region index

        [HttpGet]
        public IActionResult Index(FilterBlogsViewModel filter)
        {
            return View(_blogService.FilterBlogs(filter));
        }

        #endregion

        #region add blog

        [HttpGet("Add")]
        public IActionResult AddBlog()
        {
            return View();
        }

        [HttpPost("Add"), ValidateAntiForgeryToken]
        public IActionResult AddBlog(CreateBlogViewModel blog)
        {
            if (!ModelState.IsValid)
            {
                return View(blog);
            }

            if (_blogService.CreateBlog(blog))
            {
                TempData["SuccessMessage"] = "بلاگ با موفقیت افزوده شد";
                return RedirectToAction("Index");
            }

            TempData["ErrorMessage"] = "مشکلی در ثبت بلاگ بوجود آمد";
            return RedirectToAction("Index");
        }

        #region Get Writers

        [HttpGet("get-auto-users")]
        public IActionResult GetWriters(string writer)
        {
            var data = _userService.GetUsersForAutoComplete().ToList();
            return new JsonResult(data);
        }

        #endregion

        #endregion

        #region delete blog

        [HttpGet("Delete/{blogId}")]
        public async Task<IActionResult> DeleteBlog(int blogId)
        {
            if (await _blogService.DeleteBlog(blogId))
            {
                TempData["SuccessMessage"] = "بلاگ با موفقیت حذف شد";
                return RedirectToAction("Index");
            }
            TempData["ErrorMessage"] = "مشکلی در حذف بلاگ بوجود آمد";
            return RedirectToAction("Index");
        }

        #endregion

        #region edit blog

        [HttpGet("Edit/{blogId}")]
        public IActionResult EditBlog(int blogId)
        {
            return View(_blogService.GetBlogForEdit(blogId));
        }

        [HttpPost("Edit/{blogId}"), ValidateAntiForgeryToken]
        public async Task<IActionResult> EditBlog(EditBlogViewModel blog)
        {
            if (!ModelState.IsValid)
            {
                return View(blog);
            }

            var res = await _blogService.EditBlog(blog);

            switch (res)
            {
                case BlogEditResult.Success:
                    TempData["SuccessMessage"] = "بلاگ با موفقیت ویراایش شد";
                    return RedirectToAction("Index");
                case BlogEditResult.Failed:
                    TempData["ErrorMessage"] = "مشکلی در ویرایش بلاگ بوجود آمد";
                    return RedirectToAction("Index");
                case BlogEditResult.UnHandledError:
                    TempData["ErrorMessage"] = "خطای غیر منتظره ای رخ داد";
                    return RedirectToAction("Index");
                case BlogEditResult.NotFound:
                    return BadRequest();
                default:
                    return View(blog);
            }
        }

        #endregion
    }
}
