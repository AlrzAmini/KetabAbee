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

        public async Task<bool> AddAudioBookQAnswer(ABook_QAnswer answer)
        {
            try
            {
                await _context.ABookQAnswers.AddAsync(answer);
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

        public async Task<bool> DeleteAudioBookQAnswer(ABook_QAnswer answer)
        {
            try
            {
                answer.IsDelete = true;
                _context.ABookQAnswers.Update(answer);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
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

        public async Task<ABook_QAnswer> GetAnswerById(int answerId)
        {
            return await _context.ABookQAnswers.FindAsync(answerId);
        }

        public async Task<AudioBook> GetAudioBookById(int audiobookId)
        {
            return await _context.AudioBooks.FindAsync(audiobookId);
        }

        public async Task<int> GetAudioBookDownloadCount(int audiobookId)
        {
            return await _context.DownloadedAudioBooks.CountAsync(a => a.AudioBookId == audiobookId);
        }

        public async Task<IQueryable<ABook_QAnswer>> GetAudioBookQuestionAnswer(int questionId)
        {
            return await Task.FromResult(_context.ABookQAnswers
                .Include(a => a.User)
                .Include(a => a.Question)
                .Where(a => a.QuestionId == questionId)
                .OrderByDescending(a => a.SendDate)
                .AsQueryable());
            //todo : add view model and service for show and delete q and a
        }

        public async Task<ABook_Question> GetAudioBookQuestionById(int questionId)
        {
            return await _context.ABookQuestions.FindAsync(questionId);
        }

        public async Task<IQueryable<ABook_Question>> GetAudioBookQuestions(int audiobookId)
        {
            return await Task.FromResult(_context.ABookQuestions
                    .Include(q => q.User)
                .Where(q => q.AudioBookId == audiobookId)
                    .OrderByDescending(a => a.SendDate)
                    .AsQueryable());
        }

        public async Task<int> GetAudioBookQuestionsCount(int audiobookId)
        {
            return await _context.ABookQuestions.CountAsync(q => q.AudioBookId == audiobookId);
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

        public ABook_QAnswer GetQAnswerById(int answerId)
        {
            return _context.ABookQAnswers.Find(answerId);
        }

        public ABook_Question GetQuestionById(int questionId)
        {
            return _context.ABookQuestions.Find(questionId);
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

        public bool IsUserSendAnswer(int userId, string userIp, int answerId)
        {
            var answer = GetQAnswerById(answerId);
            if (answer == null)
            {
                return false;
            }

            if (userId == 0)
            {
                return answer.UserIp == userIp;
            }

            return answer.UserId == userId;
        }

        public bool IsUserSendQuestion(int userId, string userIp, int questionId)
        {
            var question = GetQuestionById(questionId);
            if (question == null)
            {
                return false;
            }

            if (userId == 0)
            {
                return question.UserIp == userIp;
            }

            return question.UserId == userId;
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
