using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KetabAbee.Application.Interfaces.Audio_Book;
using Microsoft.AspNetCore.Mvc;

namespace KetabAbee.Web.ViewComponents
{
    public class AudioBookQuestionsComponent : ViewComponent
    {
        private readonly IAudioBookService _audioBookService;

        public AudioBookQuestionsComponent(IAudioBookService audioBookService)
        {
            _audioBookService = audioBookService;
        }

        public async Task<IViewComponentResult> InvokeAsync(int audiobookId)
        {
            return View("AudioBookQuestionsComponent", await _audioBookService.GetABookQuestionsForShow(audiobookId));
        }
    }
}
