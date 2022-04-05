using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KetabAbee.Domain.Models.Audio_Book;

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
    }
}