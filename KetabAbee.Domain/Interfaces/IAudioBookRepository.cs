using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KetabAbee.Domain.Models.Audio_Book;
using KetabAbee.Domain.Models.Audio_Book.Q_And_A;

namespace KetabAbee.Domain.Interfaces
{
    public interface IAudioBookRepository
    {
        Task<IQueryable<AudioBook>> GetAudioBooks();

        Task<AudioBook> GetAudioBookById(int audiobookId);

        Task<bool> DeleteAudioBook(AudioBook book);

        Task<bool> UpdateAudioBook(AudioBook book);

        Task<bool> AddAudioBook(AudioBook book);

        Task AddDownloadAudioBook(DownloadedAudioBook downloadedAudioBook);

        Task<bool> IsDownloadAudioBookRepetitious(int audiobookId, string userIp);

        Task<int> GetAudioBookDownloadCount(int audiobookId);

        Task<List<AudioBook>> GetMostDownloadedAudioBooks();

        Task IncreaseAudioBookDownloadCount(AudioBook audioBook);

        Task<bool> AddAudioBookRequest(AudioBookRequest audioBookRequest);

        Task<IEnumerable<AudioBookRequest>> GetAllRequests();

        #region Q & Answer

        Task<bool> AddAudioBookQuestion(ABook_Question question);

        Task<bool> UpdateAudioBookQuestion(ABook_Question question);

        Task<bool> DeleteAudioBookQuestion(ABook_Question question);

        Task<ABook_Question> GetAudioBookQuestionById(int questionId);

        Task<int> GetAudioBookQuestionsCount(int audiobookId);

        Task<IQueryable<ABook_Question>> GetAudioBookQuestions(int audiobookId);

        Task<IQueryable<ABook_QAnswer>> GetAudioBookQuestionAnswer(int questionId);

        Task<bool> AddAudioBookQAnswer(ABook_QAnswer answer);

        Task<ABook_QAnswer> GetAnswerById(int answerId);

        ABook_QAnswer GetQAnswerById(int answerId);
        ABook_Question GetQuestionById(int questionId);

        bool IsUserSendQuestion(int userId, string userIp, int questionId);
        bool IsUserSendAnswer(int userId, string userIp, int answerId);

        Task<bool> DeleteAudioBookQAnswer(ABook_QAnswer answer);

        #endregion
    }
}