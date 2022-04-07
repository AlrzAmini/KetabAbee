using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KetabAbee.Data.Context;
using KetabAbee.Domain.Interfaces;
using KetabAbee.Domain.Models.Audio_Book;
using KetabAbee.Domain.Models.Audio_Book.Q_And_A;
using Microsoft.EntityFrameworkCore;

namespace KetabAbee.Data.Repository
{
    public class AudioBookRepository : IAudioBookRepository
    {
        private readonly KetabAbeeDBContext _context;

        public AudioBookRepository(KetabAbeeDBContext context)
        {
            _context = context;
        }

        public async Task<bool> AddAudioBook(AudioBook book)
        {
            try
            {
                await _context.AudioBooks.AddAsync(book);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> AddAudioBookQuestion(ABook_Question question)
        {
            try
            {
                await _context.ABookQuestions.AddAsync(question);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> AddAudioBookRequest(AudioBookRequest audioBookRequest)
        {
            await _context.AudioBookRequests.AddAsync(audioBookRequest);
            await _context.SaveChangesAsync();
            return true;
            //try
            //{
            //    await _context.AudioBookRequests.AddAsync(audioBookRequest);
            //    await _context.SaveChangesAsync();
            //    return true;
            //}
            //catch
            //{
            //    return false;
            //}
        }

        public async Task AddDownloadAudioBook(DownloadedAudioBook downloadedAudioBook)
        {
            await _context.DownloadedAudioBooks.AddAsync(downloadedAudioBook);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteAudioBook(AudioBook book)
        {
            book.IsDelete = true;
            return await UpdateAudioBook(book);
        }

        public async Task<bool> DeleteAudioBookQuestion(ABook_Question question)
        {
            try
            {
                question.IsDelete = true;
                await UpdateAudioBookQuestion(question);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<IEnumerable<AudioBookRequest>> GetAllRequests()
        {
            return await Task.FromResult(_context.AudioBookRequests.AsQueryable());
        }

        public async Task<AudioBook> GetAudioBookById(int audiobookId)
        {
            return await _context.AudioBooks.FindAsync(audiobookId);
        }

        public async Task<int> GetAudioBookDownloadCount(int audiobookId)
        {
            return await _context.DownloadedAudioBooks.CountAsync(a => a.AudioBookId == audiobookId);
        }

        public async Task<ABook_Question> GetAudioBookQuestionById(int questionId)
        {
            return await _context.ABookQuestions.FindAsync(questionId);
        }

        public async Task<IQueryable<AudioBook>> GetAudioBooks()
        {
            return await Task.FromResult(_context.AudioBooks.OrderByDescending(b => b.CreateDate).AsQueryable());
        }

        public async Task<List<AudioBook>> GetMostDownloadedAudioBooks()
        {
            return await _context.AudioBooks
                .OrderByDescending(b => b.DownloadCount)
                .Take(10)
                .ToListAsync();
        }

        public async Task IncreaseAudioBookDownloadCount(AudioBook audioBook)
        {
            audioBook.DownloadCount++;
            await UpdateAudioBook(audioBook);
        }

        public async Task<bool> IsDownloadAudioBookRepetitious(int audiobookId, string userIp)
        {
            return await _context.DownloadedAudioBooks.AnyAsync(d =>
                d.AudioBookId == audiobookId && d.UserIp == userIp);
        }

        public async Task<bool> UpdateAudioBook(AudioBook book)
        {
            try
            {
                _context.AudioBooks.Update(book);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateAudioBookQuestion(ABook_Question question)
        {
            try
            {
                _context.ABookQuestions.Update(question);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
