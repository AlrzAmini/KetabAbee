using System.Linq;
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

        public bool AddEmailToNewsEmails(NewsEmail email)
        {
            _context.NewsEmails.Add(email);
            _context.SaveChanges();
            return true;
        }

        public bool EmailIsUnique(string email)
        {
            return !_context.NewsEmails.Any(e => e.EmailAddress == email);
        }
    }
}
