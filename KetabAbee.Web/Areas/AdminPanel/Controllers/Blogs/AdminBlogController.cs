using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KetabAbee.Application.Const;
using KetabAbee.Application.DTOs.Admin.Blog;
using KetabAbee.Application.Interfaces.Blog;
using KetabAbee.Application.Security;

namespace KetabAbee.Web.Areas.AdminPanel.Controllers.Blogs
{
    [PermissionChecker(PerIds.AdminBlogs)]
    public class AdminBlogController : AdminBaseController
    {
        #region ctor

        private readonly IBlogService _blogService;

        public AdminBlogController(IBlogService blogService)
        {
            _blogService = blogService;
        }

        #endregion

        #region index

        [HttpGet]
        public IActionResult Index(FilterBlogsViewModel filter)
        {
            return View(_blogService.FilterBlogs(filter));
        }

        #endregion
    }
}
