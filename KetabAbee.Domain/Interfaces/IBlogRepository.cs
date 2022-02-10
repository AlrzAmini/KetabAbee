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

        bool DeleteBlog(Blog blog);

        bool UpdateBlog(Blog blog);

        Blog GetBlogById(int blogId);
    }
}
