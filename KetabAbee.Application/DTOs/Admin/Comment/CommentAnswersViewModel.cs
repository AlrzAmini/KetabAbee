using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KetabAbee.Application.DTOs.Admin.Comment
{
    public class CommentAnswersViewModel
    {
        public int CAnswerId { get; set; }

        public int CommentId { get; set; }

        public string UserName { get; set; }

        public int UserId { get; set; }

        public string UserIp { get; set; }

        public string Email { get; set; }

        public string AnswerBody { get; set; }

        public DateTime SendDate { get; set; }
    }
}
