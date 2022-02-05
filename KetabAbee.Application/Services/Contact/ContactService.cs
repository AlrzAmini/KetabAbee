using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KetabAbee.Application.Convertors;
using KetabAbee.Application.DTOs.Admin;
using KetabAbee.Application.DTOs.Contact;
using KetabAbee.Application.Interfaces.Contact;
using KetabAbee.Application.Senders;
using KetabAbee.Domain.Interfaces;
using KetabAbee.Domain.Models.ContactUs;

namespace KetabAbee.Application.Services.Contact
{
    public class ContactService : IContactService
    {
        private readonly IContactRepository _contactRepository;
        
        public ContactService(IContactRepository contactRepository)
        {
            _contactRepository = contactRepository;
        }

        public bool AddEmailToNewsEmails(string email)
        {
            email = FixText.EmailFixer(email);
            return _contactRepository.AddEmailToNewsEmails(new NewsEmail { EmailAddress = email });
        }

        public bool AddNewsLetter(CreateNewsLetterViewModel letter)
        {
            var newNewsLetter = new NewsLetter
            {
                SendDate = DateTime.Now,
                Body = letter.Body,
                Subject = letter.Subject,
            };
            return _contactRepository.AddNewsLetter(newNewsLetter);
        }

        public bool CreateContactUs(CreateContactUsViewModel contactUs, string userIp, int? userId)
        {
            var newContactUs = new ContactUs
            {
                UserId = userId,
                UserIp = userIp,
                UserName = contactUs.UserName,
                Email = contactUs.Email,
                Body = contactUs.Body,
                Subject = contactUs.Subject,
                SendDate = DateTime.Now
            };
            return _contactRepository.AddContactUs(newContactUs);
        }

        public bool EmailInNewsEmailsIsUnique(string email)
        {
            return _contactRepository.EmailIsUnique(FixText.EmailFixer(email));
        }

        public IEnumerable<NewsLetter> GetNewsLetters()
        {
            return _contactRepository.GetNewsLetters();
        }

        public bool SendNewsLetterToAll(int newsId)
        {
            try
            {
                var news = _contactRepository.GetNewsLetterById(newsId);

                foreach (var email in _contactRepository.GetNewsEmailEmails())
                {
                    SendEmail.Send(email, news.Subject, news.Body);
                }

                news.IsSend = true;
                return _contactRepository.UpdateNewsLetter(news);
            }
            catch
            {
                return false;
            }
        }
    }
}
