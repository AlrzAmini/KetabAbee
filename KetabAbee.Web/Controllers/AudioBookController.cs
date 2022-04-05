using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KetabAbee.Application.Interfaces.Audio_Book;

namespace KetabAbee.Web.Controllers
{
    public class AudioBookController : Controller
    {
        #region constructor

        private readonly IAudioBookService _audioBookService;

        public AudioBookController(IAudioBookService audioBookService)
        {
            _audioBookService = audioBookService;
        }

        #endregion

        #region index

        public async Task<IActionResult> AudioBooks()
        {
            return View(await _audioBookService.GetAllAudioBooksForShow());
        }

        #endregion

        #region audio book info

        [HttpGet("A-Book/{audiobookId}/{audiobookName}")]
        [HttpGet("A-B/{audiobookId}")]
        public async Task<IActionResult> AudioBookInfo(int audiobookId, string audiobookName)
        {
            var model = await _audioBookService.GetAudioBookForShowById(audiobookId);
            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }

        #endregion

        #region player

        //[HttpGet("Play/{audiobookId}")]
        //public async Task<IActionResult> ShowPlayer(int audiobookId)
        //{
        //    var playerInfo = await _audioBookService.GetPlayerInfoForShow(audiobookId);
        //    if (playerInfo == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(playerInfo);
        //}

        #endregion
    }
}
