using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KetabAbee.Application.Const;
using KetabAbee.Application.DTOs.Admin.AudioBook;
using KetabAbee.Application.Extensions;
using KetabAbee.Application.Interfaces.Audio_Book;
using KetabAbee.Application.Security;

namespace KetabAbee.Web.Areas.AdminPanel.Controllers.Audio_Book
{
    [PermissionChecker(PerIds.AdminAudioBooks)]
    [Route("Admin/AudioBooks")]
    public class AdminAudioBookController : AdminBaseController
    {
        #region constructor

        private readonly IAudioBookService _audioBookService;

        public AdminAudioBookController(IAudioBookService service)
        {
            _audioBookService = service;
        }

        #endregion

        #region audio book list

        public async Task<IActionResult> Index(FilterAudioBooksViewModel filter)
        {
            return View(await _audioBookService.FilterAudioBooks(filter));
        }

        #endregion

        #region create audio book

        [HttpGet("Upsert")]
        public async Task<IActionResult> UpsertAudioBook(int? audiobookId)
        {
            if (audiobookId == null) return View((CreateAndEditAudioBookViewModel)null);

            var model = await _audioBookService.GetAudioBookForUpsertById((int)audiobookId);
            return View(model);
        }

        [HttpPost("Upsert")]
        public async Task<IActionResult> UpsertAudioBook(CreateAndEditAudioBookViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            // create
            if (model.AudioBookId == 0)
            {
                var res = await _audioBookService.AddAudioBook(model);
                switch (res)
                {
                    case CreateAudioBookResult.Success:
                        TempData["SuccessMessage"] = "با موفقیت افزوده شد";
                        return RedirectToAction("Index");
                    case CreateAudioBookResult.ValidationFileError:
                        ModelState.AddModelError("File", "فرمت انتخاب شده برای فایل صوتی معتبر نیست لطفا فقط فایل صوتی انتخاب کنید");
                        return View(model);
                    case CreateAudioBookResult.Error:
                        TempData["ErrorMessage"] = "عملیات با شکست مواجه شد";
                        return RedirectToAction("Index");
                    default:
                        TempData["ErrorMessage"] = "عملیات با شکست مواجه شد";
                        return RedirectToAction("Index");
                }
            }

            // edit
            var result = await _audioBookService.EditAudioBook(model);
            switch (result)
            {
                case EditAudioBookResult.Success:
                    TempData["SuccessMessage"] = "با موفقیت ویرایش شد";
                    return RedirectToAction("Index");
                case EditAudioBookResult.ValidationFileError:
                    ModelState.AddModelError("File", "فرمت انتخاب شده برای فایل صوتی معتبر نیست لطفا فقط فایل صوتی انتخاب کنید");
                    return View(model);
                case EditAudioBookResult.NotFound:
                    TempData["ErrorMessage"] = "کتاب صوتی مورد نظر یافت نشد";
                    return RedirectToAction("Index");
                case EditAudioBookResult.Error:
                    TempData["ErrorMessage"] = "عملیات با شکست مواجه شد";
                    return RedirectToAction("Index");
                default:
                    TempData["ErrorMessage"] = "عملیات با شکست مواجه شد";
                    return RedirectToAction("Index");
            }
        }

        #endregion

        #region delete audio book

        [HttpGet("Delete/{audiobookId}")]
        public async Task<IActionResult> DeleteAudioBook(int audiobookId)
        {
            if (await _audioBookService.DeleteAudioBook(audiobookId))
            {
                TempData["SuccessMessage"] = "با موفقیت حذف شد";
                return RedirectToAction("Index");
            }
            TempData["ErrorMessage"] = "عملیات با شکست مواجه شد";
            return RedirectToAction("Index");
        }

        #endregion
    }
}
