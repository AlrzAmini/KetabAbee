using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KetabAbee.Application.Convertors;
using KetabAbee.Application.DTOs.Admin;
using KetabAbee.Application.DTOs.Admin.Contact;
using KetabAbee.Application.DTOs.Contact;
using KetabAbee.Application.DTOs.Paging;
using KetabAbee.Application.Extensions;
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

        public async Task<bool> AddEmailToNewsEmails(string email)
        {
            email = email.Sanitizer();
            return await _contactRepository.AddEmailToNewsEmails(new NewsEmail { EmailAddress = email });
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

        public async Task<bool> AddRequestBranch(CreateRequestBranchViewModel model)
        {
            try
            {
                var reqBranch = new RequestBranch
                {
                    Name = model.Name.Sanitizer(),
                    Address = model.Address.Sanitizer(),
                    Phone = model.Phone.Sanitizer(),
                    CreateDate = DateTime.Now
                };
                return await _contactRepository.AddReqBranch(reqBranch);
            }
            catch
            {
                return false;
            }
        }

        public bool CreateContactUs(CreateContactUsViewModel contactUs, string userIp, int? userId)
        {
            var newContactUs = new ContactUs
            {
                UserId = userId,
                UserIp = userIp,
                UserName = contactUs.UserName.Sanitizer(),
                Email = contactUs.Email.Sanitizer(),
                Body = contactUs.Body.Sanitizer(),
                Subject = contactUs.Subject.Sanitizer(),
                SendDate = DateTime.Now
            };
            return _contactRepository.AddContactUs(newContactUs);
        }

        public bool EmailInNewsEmailsIsUnique(string email)
        {
            return _contactRepository.EmailIsUnique(FixText.EmailFixer(email));
        }

        public IEnumerable<ContactUs> GetContactUses()
        {
            return _contactRepository.GetContactUses();
        }

        public ContactUsesForAdminViewModel GetContactUsesForShowInAdmin(ContactUsesForAdminViewModel model)
        {
            var result = GetContactUses().AsQueryable();

            //paging
            var pager = Pager.Build(model.PageNum, result.Count(), model.Take, model.PageCountAfterAndBefor);
            var contactUses = result.Paging(pager).ToList();

            return model.SetPaging(pager).SetContactUses(contactUses);
        }

        public IEnumerable<NewsEmail> GetNewsLetterEmails()
        {
            return _contactRepository.GetNewsLetterEmails();
        }

        public IEnumerable<NewsLetter> GetNewsLetters()
        {
            return _contactRepository.GetNewsLetters();
        }

        public ShowBranchRequestsToAdminViewModel GetRequestsForShowAdmin(ShowBranchRequestsToAdminViewModel model)
        {
            var result = _contactRepository.GetRequests().AsQueryable();

            //paging
            var pager = Pager.Build(model.PageNum, result.Count(), model.Take, model.PageCountAfterAndBefor);
            var requests = result.Paging(pager).ToList();

            return model.SetPaging(pager).SetRequests(requests);
        }

        public async Task<bool> SendAnswerForContactUs(int contactId, string subject, string body)
        {
            try
            {
                var contact = _contactRepository.GetContactUsById(contactId);
                if (contact == null)
                {
                    return false;
                }
                await SendEmail.Send(contact.Email, subject, body);
                contact.IsAnswered = true;
                return _contactRepository.UpdateContactUs(contact);
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> SendNewsLetterToAll(int newsId)
        {
            try
            {
                var news = _contactRepository.GetNewsLetterById(newsId);

                foreach (var email in _contactRepository.GetNewsEmailEmails())
                {
                    await SendEmail.Send(email, news.Subject, news.Body);
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
