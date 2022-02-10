using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KetabAbee.Application.DTOs.Admin.Blog;

namespace KetabAbee.Application.Interfaces.Blog
{
    public interface IBlogService
    {
        IEnumerable<ShowBlogInListViewModel> GetBlogsForShowInAdmin();

        FilterBlogsViewModel FilterBlogs(FilterBlogsViewModel filter);
    }
}
