using System.Collections.Generic;
using KetabAbee.Domain.Models.ContactUs;

namespace KetabAbee.Domain.Interfaces
{
    public interface IContactRepository
    {
        #region news letter

        bool AddEmailToNewsEmails(NewsEmail email);

        bool EmailIsUnique(string email);

        IEnumerable<string> GetNewsEmailEmails();

        IEnumerable<NewsLetter> GetNewsLetters();

        IEnumerable<NewsEmail> GetNewsLetterEmails();

        bool AddNewsLetter(NewsLetter letter);

        NewsLetter GetNewsLetterById(int newsId);

        bool UpdateNewsLetter(NewsLetter letter);

        #endregion

        #region Contact us

        bool AddContactUs(ContactUs contactUs);

        IEnumerable<ContactUs> GetContactUses();

        ContactUs GetContactUsById(int contactId);

        #endregion
    }
}
