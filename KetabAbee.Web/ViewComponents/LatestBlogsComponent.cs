using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KetabAbee.Application.Interfaces.Blog;
using Microsoft.AspNetCore.Mvc;

namespace KetabAbee.Web.ViewComponents
{
    public class LatestBlogsComponent : ViewComponent
    {
        private readonly IBlogService _blogService;

        public LatestBlogsComponent(IBlogService blogService)
        {
            _blogService = blogService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return await Task.FromResult((IViewComponentResult)View("LatestBlogs", _blogService.GetBlogsCardInfo().Take(6).ToList()));
        }
    }
}
