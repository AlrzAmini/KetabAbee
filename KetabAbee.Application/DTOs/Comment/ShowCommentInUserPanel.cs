using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KetabAbee.Application.DTOs.Comment
{
    public class ShowCommentInUserPanel
    {
        public string UserName { get; set; }

        public string Body { get; set; }

        public DateTime SendDate { get; set; }

        public int Id { get; set; }
    }
}
