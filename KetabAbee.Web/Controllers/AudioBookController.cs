using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using KetabAbee.Application.DTOs.AudioBook;
using KetabAbee.Application.DTOs.AudioBook.QA.Question;
using KetabAbee.Application.Extensions;
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

        [HttpGet("A-Books")]
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

        #region download

        [HttpGet("DownloadFile/{audiobookId}")]
        public async Task<IActionResult> DownloadFile(int audiobookId)
        {
            // if download was unique add a download to download table
            // for counting most downloaded audio book and users favorites
            var userIp = HttpContext.GetUserIp();
            if (!await _audioBookService.IsDownloadAudioBookRepetitious(audiobookId, userIp))
            {
                await _audioBookService.IncreaseAudioBookDownloadCount(audiobookId);
                await _audioBookService.AddDownloadAudioBook(audiobookId, userIp);
            }

            // download file
            var fileInfo = await _audioBookService.GetFileInfoById(audiobookId);
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot" + fileInfo.FileSavePath);
            var fileName = fileInfo.FileName;

            var file = await System.IO.File.ReadAllBytesAsync(filePath);
            return File(file, "application/force-download", fileName);
        }

        #endregion

        #region request new book

        [HttpGet("load-modal-request-book")]
        public IActionResult RequestBook(int audiobookId)
        {
            var createModel = new CreateAudioBookRequest
            {
                AudioBookId = audiobookId
            };
            return PartialView("_RequestBookModal", createModel);
        }

        [HttpPost("add-request-book"), ValidateAntiForgeryToken]
        public async Task<IActionResult> RequestBook(CreateAudioBookRequest request)
        {
            if (User.Identity.IsAuthenticated)
            {
                request.UserName = User.GetUserName();
            }
            request.UserIp = HttpContext.GetUserIp();
            if (await _audioBookService.AddAudioBookRequest(request))
            {
                TempData["SuccessSwal"] = "درخواست شما با موفقیت ثبت شد";
                return RedirectToAction("AudioBookInfo", new { audiobookId = request.AudioBookId });
            }
            TempData["ErrorSwal"] = "مشکلی در ثبت درخواست رخ داد";
            return RedirectToAction("AudioBookInfo", new { audiobookId = request.AudioBookId });
        }

        #endregion

        #region question

        [HttpGet("load-modal-create-ABook-question")]
        public IActionResult LoadCreateQuestionModal(int audiobookId)
        {
            var createQuestion = new CreateQuestionViewModel
            {
                AudioBookId = audiobookId
            };
            return PartialView("_CreateQuestionModal", createQuestion);
        }

        [HttpPost("CreateQuestion"), ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateQuestion(CreateQuestionViewModel question)
        {
            if (User.Identity.IsAuthenticated)
            {
                question.UserName = User.GetUserName();
            }

            question.UserIp = HttpContext.GetUserIp();
            question.UserId = User.GetUserId();
            if (await _audioBookService.CreateQuestion(question))
            {
                TempData["SuccessSwal"] = "پرسش شما با موفقیت ثبت شد";
                return RedirectToAction("AudioBookInfo", new { audiobookId = question.AudioBookId });
            }
            TempData["ErrorSwal"] = "مشکلی در اننجام عملیات رخ داد";
            return RedirectToAction("AudioBookInfo", new { audiobookId = question.AudioBookId });
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
