using System;

namespace KetabAbee.Application.DTOs.Admin.Blog
{
    public class ShowBlogInListViewModel
    {
        #region props

        public int BlogId { get; set; }

        public string BlogTitle { get; set; }

        public string Writer { get; set; }

        public string ImageName { get; set; }

        public DateTime CreateDate { get; set; }

        #endregion
    }
}
