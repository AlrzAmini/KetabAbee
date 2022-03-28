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

        Task<bool> DeleteBlog(int blogId);

        Task<bool> UpdateBlog(int blogId);

        EditBlogViewModel GetBlogForEdit(int blogId);

        Task<BlogEditResult> EditBlog(EditBlogViewModel blog);

        FilterBlogsIndexViewModel FilterBlogsInIndex(FilterBlogsIndexViewModel filter);

        ShowBlogInfoViewModel GetBlogForShowInBlogInfo(int blogId);

        List<BlogInCardViewModel> GetWriterBlogs(int userId);

        Task<bool> IncreaseBlogCount(int blogId, string userIp, int? userId);

        Task<bool> IsBlogViewExist(int blogId, string userIp, int? userId);

        Task<int> BlogViewCount(int blogId);
    }
}
