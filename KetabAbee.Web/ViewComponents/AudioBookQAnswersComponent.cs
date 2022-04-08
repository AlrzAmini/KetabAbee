using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KetabAbee.Application.Extensions;
using KetabAbee.Application.Interfaces.Audio_Book;
using Microsoft.AspNetCore.Mvc;

namespace KetabAbee.Web.ViewComponents
{
    public class AudioBookQAnswersComponent : ViewComponent
    {
        private readonly IAudioBookService _audioBookService;

        public AudioBookQAnswersComponent(IAudioBookService audioBookService)
        {
            _audioBookService = audioBookService;
        }

        public async Task<IViewComponentResult> InvokeAsync(int questionId)
        {
            return View("AudioBookQAnswers", await _audioBookService.GetQuestionAnswersForShow(questionId,HttpContext.User.GetUserId(),HttpContext.GetUserIp()));
        }
    }
}
