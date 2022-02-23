using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KetabAbee.Application.DTOs.Admin.Comment
{
    public class CommentInAdminViewModel
    {
        public int CommentId { get; set; }

        public string SenderName { get; set; }

        public DateTime SendDate { get; set; }

        public string Body { get; set; }

        public string ProductName { get; set; }

        public bool IsReadByAdmin { get; set; }
    }

    public enum ChangeCommentIsReadResult
    {
        ChangedToRead,
        ChangedToIsNotRead,
        Error,
        NotFound
    }
}
