using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KetabAbee.Application.DTOs.AudioBook.QA.Answer
{
    public class ShowABookAnswersViewModel
    {
        public int AnswerId { get; set; }

        public int QuestionId { get; set; }

        public int AudioBookId { get; set; }

        public string UserName { get; set; }

        public string Body { get; set; }

        public bool IsCurrentUserSentAnswer { get; set; }

        public string AvatarName { get; set; }

        public DateTime SendDate { get; set; }
    }
}
