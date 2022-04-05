using System.Collections.Generic;
using System.Threading.Tasks;
using KetabAbee.Application.DTOs.Admin.AudioBook;
using KetabAbee.Application.DTOs.AudioBook;

namespace KetabAbee.Application.Interfaces.Audio_Book
{
    public interface IAudioBookService
    {
        Task<FilterAudioBooksViewModel> FilterAudioBooks(FilterAudioBooksViewModel filter);

        Task<CreateAndEditAudioBookViewModel> GetAudioBookForUpsertById(int audiobookId);

        Task<bool> DeleteAudioBook(int audiobookId);

        Task<CreateAudioBookResult> AddAudioBook(CreateAndEditAudioBookViewModel model);

        Task<EditAudioBookResult> EditAudioBook(CreateAndEditAudioBookViewModel model);

        Task<List<AudioBookBoxViewModel>> GetAllAudioBooksForShow();

        Task<ShowAudioBookInfoViewModel> GetAudioBookForShowById(int audiobookId);

        Task<ShowPlayerViewModel> GetPlayerInfoForShow(int audiobookId);
    }
}