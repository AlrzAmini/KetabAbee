using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KetabAbee.Data.Context;
using KetabAbee.Domain.Interfaces;
using KetabAbee.Domain.Models.Audio_Book;

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

        public async Task<bool> DeleteAudioBook(AudioBook book)
        {
            book.IsDelete = true;
            return await UpdateAudioBook(book);
        }

        public async Task<AudioBook> GetAudioBookById(int audiobookId)
        {
            return await _context.AudioBooks.FindAsync(audiobookId);
        }

        public async Task<IQueryable<AudioBook>> GetAudioBooks()
        {
            return await Task.FromResult(_context.AudioBooks.AsQueryable());
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
    }
}
