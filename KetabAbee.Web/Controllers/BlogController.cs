using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KetabAbee.Application.DTOs.Admin.Blog;
using KetabAbee.Application.DTOs.Blog;
using KetabAbee.Application.Interfaces.Blog;

namespace KetabAbee.Web.Controllers
{
    [Route("Blogs")]
    public class BlogController : Controller
    {
        #region ctor

        private readonly IBlogService _blogService;

        public BlogController(IBlogService blogService)
        {
            _blogService = blogService;
        }

        #endregion

        [HttpGet]
        public IActionResult Index(FilterBlogsIndexViewModel filter)
        {
            return View(_blogService.FilterBlogsInIndex(filter));
        }
    }
}
