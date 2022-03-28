using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KetabAbee.Domain.Models.Blog;

namespace KetabAbee.Domain.Interfaces
{
    public interface IBlogRepository
    {
        IEnumerable<Blog> GetBlogs();

        bool AddBlog(Blog blog);

        Task<bool> DeleteBlog(Blog blog);

        Task<bool> UpdateBlog(Blog blog);

        List<Blog> GetWriterBlogs(int userId);

        Task<bool> AddBlogView(BlogView view);

        Task<bool> IsBlogViewExist(int blogId, string userIp);
        Task<bool> IsBlogViewExist(int blogId, int userId);

        Task<Blog> GetBlogById(int blogId);
    }
}
