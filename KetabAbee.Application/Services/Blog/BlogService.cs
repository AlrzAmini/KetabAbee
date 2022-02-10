using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KetabAbee.Application.DTOs.Admin.Blog;
using KetabAbee.Application.DTOs.Paging;
using KetabAbee.Application.Interfaces.Blog;
using KetabAbee.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace KetabAbee.Application.Services.Blog
{
    public class BlogService : IBlogService
    {
        private readonly IBlogRepository _blogRepository;

        public BlogService(IBlogRepository blogRepository)
        {
            _blogRepository = blogRepository;
        }

        public FilterBlogsViewModel FilterBlogs(FilterBlogsViewModel filter)
        {
            var result = GetBlogsForShowInAdmin().AsQueryable();

            #region filter by title

            if (!string.IsNullOrEmpty(filter.BlogTitle))
            {
                result = result.Where(r => EF.Functions.Like(r.BlogTitle, $"%{filter.BlogTitle}%"));
            }

            #endregion

            #region filter by writer

            if (!string.IsNullOrEmpty(filter.Writer))
            {
                result = result.Where(r => EF.Functions.Like(r.Writer, $"%{filter.Writer}%"));
            }

            #endregion

            #region paging

            //paging
            var pager = Pager.Build(filter.PageNum, result.Count(), filter.Take, filter.PageCountAfterAndBefor);
            var blogs = result.Paging(pager).ToList();

            return filter.SetPaging(pager).SetBlogs(blogs);

            #endregion
        }

        public IEnumerable<ShowBlogInListViewModel> GetBlogsForShowInAdmin()
        {
            return _blogRepository.GetBlogs()
                .Select(b => new ShowBlogInListViewModel
                {
                    ImageName = b.ImageName,
                    CreateDate = b.CreateDate,
                    BlogId = b.BlogId,
                    BlogTitle = b.BlogTitle,
                    Writer = b.User.UserName
                });
        }
    }
}
