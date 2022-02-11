using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KetabAbee.Application.DTOs.Admin.Blog;
using KetabAbee.Application.DTOs.Blog;

namespace KetabAbee.Application.Interfaces.Blog
{
    public interface IBlogService
    {
        IEnumerable<ShowBlogInListViewModel> GetBlogsForShowInAdmin();

        FilterBlogsViewModel FilterBlogs(FilterBlogsViewModel filter);

        bool CreateBlog(CreateBlogViewModel blog);

        IEnumerable<BlogInCardViewModel> GetBlogsCardInfo();

        bool DeleteBlog(int blogId);

        bool UpdateBlog(int blogId);

        Domain.Models.Blog.Blog GetBlogById(int blogId);

        EditBlogViewModel GetBlogForEdit(int blogId);

        BlogEditResult EditBlog(EditBlogViewModel blog);

        FilterBlogsIndexViewModel FilterBlogsInIndex(FilterBlogsIndexViewModel filter);

        ShowBlogInfoViewModel GetBlogForShowInBlogInfo(int blogId);

        List<BlogInCardViewModel> GetWriterBlogs(int userId);
    }
}
