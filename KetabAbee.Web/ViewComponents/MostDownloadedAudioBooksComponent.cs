using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KetabAbee.Application.Interfaces.Audio_Book;
using Microsoft.AspNetCore.Mvc;

namespace KetabAbee.Web.ViewComponents
{
    public class MostDownloadedAudioBooksComponent : ViewComponent
    {
        private readonly IAudioBookService _audioBookService;

        public MostDownloadedAudioBooksComponent(IAudioBookService audioBookService)
        {
            _audioBookService = audioBookService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View("MostDownloadedAudioBooks", await _audioBookService.GetMostDownloadedAudioBooks());
        }
    }
}
