using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KetabAbee.Application.DTOs.Paging;

namespace KetabAbee.Application.DTOs.Blog
{
    public class FilterBlogsIndexViewModel : BasePaging
    {
        public string BlogTitle { get; set; }

        public string Tag { get; set; }

        public List<BlogInCardViewModel> Blogs { get; set; }

        #region Methods

        public FilterBlogsIndexViewModel SetBlogs(List<BlogInCardViewModel> blogs)
        {
            Blogs = blogs;
            return this;
        }

        public FilterBlogsIndexViewModel SetPaging(BasePaging paging)
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
