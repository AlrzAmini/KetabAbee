using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KetabAbee.Application.Interfaces.Audio_Book;
using Microsoft.AspNetCore.Mvc;

namespace KetabAbee.Web.ViewComponents
{
    public class AudioBooksComponent : ViewComponent
    {
        private readonly IAudioBookService _audioBookService;

        public AudioBooksComponent(IAudioBookService audioBookService)
        {
            _audioBookService = audioBookService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View("AudioBooks", await _audioBookService.GetAllAudioBooksForShow());
        }
    }
}
