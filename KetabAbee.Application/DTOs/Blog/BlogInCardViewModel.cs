using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KetabAbee.Application.DTOs.Blog
{
   public class BlogInCardViewModel
    {
        #region props

        public int BlogId { get; set; }

        public string BlogTitle { get; set; }

        public string BlogDescription { get; set; }

        public string Writer { get; set; }

        public string ImageName { get; set; }

        public DateTime CreateDate { get; set; }

        public string WriterAvatarName { get; set; }

        public string Tags { get; set; }

        public int ViewsCount { get; set; }

        #endregion
    }
}
