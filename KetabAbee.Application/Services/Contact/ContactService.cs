using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KetabAbee.Application.Convertors;
using KetabAbee.Application.DTOs.Contact;
using KetabAbee.Application.Interfaces.Contact;
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
    }
}
