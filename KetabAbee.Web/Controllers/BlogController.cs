﻿using Microsoft.AspNetCore.Mvc;
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

        #region index

        [HttpGet]
        public IActionResult Index(FilterBlogsIndexViewModel filter)
        {
            return View(_blogService.FilterBlogsInIndex(filter));
        }

        #endregion

        #region blog info

        [HttpGet("Blog/{blogId}")]
        public IActionResult BlogInfo(int blogId)
        {
            var model = _blogService.GetBlogForShowInBlogInfo(blogId);

            if (model == null)
            {
                return NotFound();
            }

            ViewBag.Tags = model.Tags.Split('-')
                .Where(t=>t.Length > 1).ToArray();
            
            return View(model);
        }

        #endregion
    }
}
