using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KetabAbee.Application.DTOs.Admin.Products.Options
{
    public class ProductCommentForShowInAdminBookInfoViewModel
    {
        public int CommentId { get; set; }

        public string ProductName { get; set; }

        public string SenderName { get; set; }

        public int? SenderId { get; set; }

        public string SenderEmail { get; set; }

        public string SenderIp { get; set; }

        public string CommentBody { get; set; }

        public DateTime SendDate { get; set; }

        public bool IsReadByAdmin { get; set; }
    }
}
