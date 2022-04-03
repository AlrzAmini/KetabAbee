using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KetabAbee.Application.Convertors;
using KetabAbee.Application.DTOs.Admin.AudioBook;
using KetabAbee.Application.DTOs.Paging;
using KetabAbee.Application.Extensions;
using KetabAbee.Application.Generators;
using KetabAbee.Application.Interfaces.Audio_Book;
using KetabAbee.Domain.Interfaces;
using KetabAbee.Domain.Models.Audio_Book;

namespace KetabAbee.Application.Services.Audio_Book
{
    public class AudioBookService : IAudioBookService
    {
        private readonly IAudioBookRepository _audioBookRepository;

        public AudioBookService(IAudioBookRepository audioBookRepository)
        {
            _audioBookRepository = audioBookRepository;
        }

        public async Task<CreateAudioBookResult> AddAudioBook(CreateAndEditAudioBookViewModel model)
        {
            var newAudioBook = new AudioBook
            {
                Writer = model.Writer,
                PageDescription = model.PageDescription,
                FileSize = model.FileSize,
                Review = model.Review,
                Speaker = model.Speaker,
                Title = model.Title,
                ImageName = "Default.jpg"
            };

            #region add image

            if (model.Image != null)
            {
                // generate unique name
                newAudioBook.ImageName = CodeGenerator.GenerateUniqCode() + Path.GetExtension(model.Image.FileName);
                // select save path 
                var imgPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot" + PathExtensions.AudioBookImageFullAddress(newAudioBook.ImageName));
                // save in path
                await using (var stream = new FileStream(imgPath, FileMode.Create))
                {
                    await model.Image.CopyToAsync(stream);
                }
                var imgReSizer = new ImageConvertor();
                var imgThumbPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot" + PathExtensions.AudioBookThumbFullAddress(newAudioBook.ImageName));
                imgReSizer.Image_resize(imgPath, imgThumbPath, 400);
            }

            #endregion

            #region add file

            if (model.File != null)
            {
                if (!model.File.IsValidAudio())
                {
                    return CreateAudioBookResult.ValidationFileError;
                }
                // generate unique name
                newAudioBook.FileName = CodeGenerator.GenerateUniqCode() + Path.GetExtension(model.File.FileName);
                // select save path 
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot" + PathExtensions.AudioBookFileFullAddress(newAudioBook.FileName));
                // save in path
                await using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.File.CopyToAsync(stream);
                }
            }

            #endregion

            if (await _audioBookRepository.AddAudioBook(newAudioBook))
            {
                return CreateAudioBookResult.Success;
            }

            return CreateAudioBookResult.Error;
        }

        public async Task<bool> DeleteAudioBook(int audiobookId)
        {
            var audioBook = await _audioBookRepository.GetAudioBookById(audiobookId);
            if (audioBook == null)
            {
                return false;
            }

            #region delete image

            if (audioBook.ImageName != null && audioBook.ImageName != "Default.jpg")
            {
                var imgDeletePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot" + PathExtensions.AudioBookImageFullAddress(audioBook.ImageName));
                if (File.Exists(imgDeletePath))
                {
                    File.Delete(imgDeletePath);
                }

                #region delete thumb

                var thumbDeletePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot" + PathExtensions.AudioBookThumbFullAddress(audioBook.ImageName));
                if (File.Exists(thumbDeletePath))
                {
                    File.Delete(thumbDeletePath);
                }

                #endregion
            }

            #endregion

            #region delete file

            if (audioBook.FileName != null)
            {
                var fileDeletePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot" + PathExtensions.AudioBookFileFullAddress(audioBook.FileName));
                if (File.Exists(fileDeletePath))
                {
                    File.Delete(fileDeletePath);
                }
            }

            #endregion

            return await _audioBookRepository.DeleteAudioBook(audioBook);
        }

        public async Task<EditAudioBookResult> EditAudioBook(CreateAndEditAudioBookViewModel model)
        {
            var audioBook = await _audioBookRepository.GetAudioBookById(model.AudioBookId);
            if (audioBook == null)
            {
                return EditAudioBookResult.NotFound;
            }

            audioBook.FileSize = model.FileSize;
            audioBook.PageDescription = model.PageDescription;
            audioBook.Review = model.Review;
            audioBook.Speaker = model.Speaker;
            audioBook.Writer = model.Writer;
            audioBook.Title = model.Title;

            #region edit image

            if (model.Image != null)
            {
                if (model.ImageName != "Default.jpg")
                {
                    #region delete old image

                    var imgDeletePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot" + PathExtensions.AudioBookImageFullAddress(audioBook.ImageName));
                    if (File.Exists(imgDeletePath))
                    {
                        File.Delete(imgDeletePath);
                    }

                    #region delete old thumb

                    var thumbDeletePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot" + PathExtensions.AudioBookThumbFullAddress(audioBook.ImageName));
                    if (File.Exists(thumbDeletePath))
                    {
                        File.Delete(thumbDeletePath);
                    }

                    #endregion

                    #endregion

                    #region add new image

                    // generate unique name
                    audioBook.ImageName = CodeGenerator.GenerateUniqCode() + Path.GetExtension(model.Image.FileName);
                    // select save path 
                    var imgPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot" + PathExtensions.AudioBookImageFullAddress(audioBook.ImageName));
                    // save in path
                    await using (var stream = new FileStream(imgPath, FileMode.Create))
                    {
                        await model.Image.CopyToAsync(stream);
                    }

                    #region add thumb

                    var imgReSizer = new ImageConvertor();
                    var imgThumbPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot" + PathExtensions.AudioBookThumbFullAddress(audioBook.ImageName));
                    imgReSizer.Image_resize(imgPath, imgThumbPath, 400);

                    #endregion

                    #endregion
                }
            }

            #endregion

            #region edit file

            if (model.File != null)
            {
                if (!model.File.IsValidAudio())
                {
                    return EditAudioBookResult.ValidationFileError;
                }

                #region delete old file

                var fileDeletePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot" + PathExtensions.AudioBookFileFullAddress(audioBook.FileName));
                if (File.Exists(fileDeletePath))
                {
                    File.Delete(fileDeletePath);
                }

                #endregion

                #region add new file

                // generate unique name
                audioBook.FileName = CodeGenerator.GenerateUniqCode() + Path.GetExtension(model.File.FileName);
                // select save path 
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot" + PathExtensions.AudioBookFileFullAddress(audioBook.FileName));
                // save in path
                await using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.File.CopyToAsync(stream);
                }

                #endregion
            }

            #endregion

            if (await _audioBookRepository.UpdateAudioBook(audioBook))
            {
                return EditAudioBookResult.Success;
            }

            return EditAudioBookResult.Error;
        }


        // ToDo : CRUD Audio Book 
        public async Task<FilterAudioBooksViewModel> FilterAudioBooks(FilterAudioBooksViewModel filter)
        {
            #region query

            var query = await _audioBookRepository.GetAudioBooks();
            var result = query
                .Select(q => new ShowAudioBookInListViewModel
                {
                    Writer = q.Writer,
                    CreateDate = q.CreateDate,
                    Title = q.Title,
                    Speaker = q.Speaker,
                    ImageLocation = PathExtensions.AudioBookImageFullAddress(q.ImageName),
                    FileSize = q.FileSize,
                    AudioBookId = q.AudioBookId
                });

            #endregion

            #region filter by title

            if (!string.IsNullOrEmpty(filter.Title))
            {
                result = result.Where(r => r.Title.Contains(filter.Title));
            }

            #endregion

            #region filter by writer

            if (!string.IsNullOrEmpty(filter.Writer))
            {
                result = result.Where(r => r.Writer.Contains(filter.Writer));
            }

            #endregion

            #region filter by speaker

            if (!string.IsNullOrEmpty(filter.Speaker))
            {
                result = result.Where(r => r.Speaker.Contains(filter.Speaker));
            }

            #endregion

            #region filter by size

            if (filter.MinFileSize != 0)
            {
                result = result.Where(r => r.FileSize > filter.MinFileSize);
            }

            if (filter.MaxFileSize != 0)
            {
                result = result.Where(r => r.FileSize < filter.MaxFileSize);
            }

            #endregion

            #region paging

            var pager = Pager.Build(filter.PageNum, result.Count(), filter.Take, filter.PageCountAfterAndBefor);
            var audioBooks = result.Paging(pager).ToList();

            #endregion

            return filter.SetPaging(pager).SetAudioBooks(audioBooks);
        }

        public async Task<CreateAndEditAudioBookViewModel> GetAudioBookForUpsertById(int audiobookId)
        {
            var audioBook = await _audioBookRepository.GetAudioBookById(audiobookId);
            if (audioBook == null)
            {
                return null;
            }
            return new CreateAndEditAudioBookViewModel
            {
                ImageName = audioBook.ImageName,
                Writer = audioBook.Writer,
                PageDescription = audioBook.PageDescription,
                Title = audioBook.Title,
                Speaker = audioBook.Speaker,
                FileSize = audioBook.FileSize,
                AudioBookId = audioBook.AudioBookId,
                FileName = audioBook.FileName,
                Review = audioBook.Review
            };
        }
    }
}
