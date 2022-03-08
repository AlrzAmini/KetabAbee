using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KetabAbee.Application.DTOs.Blog
{
    public class ShowBlogInfoViewModel
    {
        #region props

        public int BlogId { get; set; }

        public int UserId { get; set; }

        public string BlogTitle { get; set; }

        public string BlogBody { get; set; }

        public string PageDescription { get; set; }

        public string Tags { get; set; }

        public DateTime CreateDate { get; set; }

        public List<BlogInCardViewModel> WriterBlogs { get; set; }

        public string UserName { get; set; }

        public string UserImageName { get; set; }

        public string ImageName { get; set; }

        public int ReadTime { get; set; }


        #endregion
    }
}
