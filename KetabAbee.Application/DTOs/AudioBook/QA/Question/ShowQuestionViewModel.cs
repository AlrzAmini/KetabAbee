using System;

namespace KetabAbee.Application.DTOs.AudioBook.QA.Question
{
    public class ShowQuestionViewModel
    {
        public int QuestionId { get; set; }

        public string Title { get; set; }

        public string UserName { get; set; }

        public string Body { get; set; }

        public bool IsCurrentUserSentQuestion { get; set; }

        public string AvatarName { get; set; }

        public DateTime SendDate { get; set; }
    }
}
