using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KetabAbee.Data.Context;
using KetabAbee.Domain.Interfaces;
using KetabAbee.Domain.Models.ContactUs;

namespace KetabAbee.Data.Repository
{
    public class ContactRepository : IContactRepository
    {
        private readonly KetabAbeeDBContext _context;

        public ContactRepository(KetabAbeeDBContext context)
        {
            _context = context;
        }

        public bool AddContactUs(ContactUs contactUs)
        {
            try
            {
                _context.ContactUses.Add(contactUs);
                _context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool AddEmailToNewsEmails(NewsEmail email)
        {
            try
            {
                _context.NewsEmails.Add(email);
                _context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool AddNewsLetter(NewsLetter letter)
        {
            try
            {
                _context.NewsLetters.Add(letter);
                _context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> AddReqBranch(RequestBranch request)
        {
            try
            {
                await _context.RequestBranches.AddAsync(request);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool EmailIsUnique(string email)
        {
            return !_context.NewsEmails.Any(e => e.EmailAddress == email);
        }

        public ContactUs GetContactUsById(int contactId)
        {
            return _context.ContactUses.Find(contactId);
        }

        public IEnumerable<ContactUs> GetContactUses()
        {
            return _context.ContactUses.OrderByDescending(c => c.SendDate);
        }

        public IEnumerable<string> GetNewsEmailEmails()
        {
            return _context.NewsEmails.Select(e => e.EmailAddress);
        }

        public NewsLetter GetNewsLetterById(int newsId)
        {
            return _context.NewsLetters.Find(newsId);
        }

        public IEnumerable<NewsEmail> GetNewsLetterEmails()
        {
            return _context.NewsEmails;
        }

        public IEnumerable<NewsLetter> GetNewsLetters()
        {
            return _context.NewsLetters;
        }

        public IEnumerable<RequestBranch> GetRequests()
        {
            return _context.RequestBranches;
        }

        public bool UpdateContactUs(ContactUs contactUs)
        {
            try
            {
                _context.ContactUses.Update(contactUs);
                _context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool UpdateNewsLetter(NewsLetter letter)
        {
            try
            {
                _context.NewsLetters.Update(letter);
                _context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
