using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KetabAbee.Application.DTOs.Admin.User
{
    public class UserProductCommentViewModel
    {

        public int CommentId { get; set; }

        public int ProductId { get; set; }

        public string ProductName { get; set; }

        public string UserIp { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public string Body { get; set; }

        public DateTime SendDate { get; set; }

        public bool IsReadByAdmin { get; set; }
    }

    public enum DeleteUserCommentsResult
    {
       Success,
       Error,
       NotHaveComment
    }
}
