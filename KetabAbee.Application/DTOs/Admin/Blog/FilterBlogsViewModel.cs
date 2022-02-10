using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KetabAbee.Application.DTOs.Paging;

namespace KetabAbee.Application.DTOs.Admin.Blog
{
    public class FilterBlogsViewModel : BasePaging
    {
        public string BlogTitle { get; set; }

        public string Writer { get; set; }

        public List<ShowBlogInListViewModel> Blogs { get; set; }

        #region Methods

        public FilterBlogsViewModel SetBlogs(List<ShowBlogInListViewModel> blogs)
        {
            Blogs = blogs;
            return this;
        }

        public FilterBlogsViewModel SetPaging(BasePaging paging)
        {
            PageNum = paging.PageNum;
            TotalEntitiesCount = paging.TotalEntitiesCount;
            StartPage = paging.StartPage;
            EndPage = paging.EndPage;
            PageCountAfterAndBefor = paging.PageCountAfterAndBefor;
            Take = paging.Take;
            Skip = paging.Skip;
            TotalPages = paging.TotalPages;
            return this;
        }

        #endregion
    }
}
