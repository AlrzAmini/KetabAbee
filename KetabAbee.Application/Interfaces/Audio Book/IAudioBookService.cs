using System.Collections.Generic;
using System.Threading.Tasks;
using KetabAbee.Application.DTOs.Admin.AudioBook;
using KetabAbee.Application.DTOs.AudioBook;
using KetabAbee.Application.DTOs.AudioBook.QA.Answer;
using KetabAbee.Application.DTOs.AudioBook.QA.Question;
using KetabAbee.Domain.Models.Audio_Book;

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

        Task<AudioBookFileInfoViewModel> GetFileInfoById(int audiobookId);

        Task<bool> IsDownloadAudioBookRepetitious(int audiobookId, string userIp);

        System.Threading.Tasks.Task AddDownloadAudioBook(int audiobookId, string userIp);

        Task<int> GetAudioBookDownloadCount(int audiobookId);

        System.Threading.Tasks.Task IncreaseAudioBookDownloadCount(int audiobookId);

        Task<List<AudioBookBoxViewModel>> GetMostDownloadedAudioBooks();

        Task<bool> AddAudioBookRequest(CreateAudioBookRequest request);

        Task<AudioBookRequestsViewModel> GetAudioBookRequestsForShowInAdmin(AudioBookRequestsViewModel request);

        #region Q & Answer

        Task<bool> CreateQuestion(CreateQuestionViewModel question);

        Task<List<ShowQuestionViewModel>> GetABookQuestionsForShow(int audiobookId, int userId, string userIp);

        Task<List<ShowABookAnswersViewModel>> GetQuestionAnswersForShow(int questionId,int userId,string userIp);

        Task<bool> CreateAnswer(CreateQAnswerViewModel answer);

        Task<int> GetAudioBookIdByQuestionId(int questionId);

        bool IsUserSendQuestion(int userId,string userIp,int questionId);
        bool IsUserSendAnswer(int userId,string userIp,int answerId);

        Task<bool> DeleteQuestionById(int questionId, int userId, string userIp);

        Task<bool> DeleteAnswerById(int answerId, int userId, string userIp);

        #endregion
    }
}