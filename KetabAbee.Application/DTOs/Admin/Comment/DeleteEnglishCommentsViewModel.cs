using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KetabAbee.Application.DTOs.Admin.Comment
{
    public class DeleteEnglishCommentsViewModel
    {
        public int CommentId { get; set; }

        public string Body { get; set; }
    }

    public enum DeleteEnglishCommentsResult
    {
        Success,
        NotFoundAnyEnglishComment,
        Error
    }
}
