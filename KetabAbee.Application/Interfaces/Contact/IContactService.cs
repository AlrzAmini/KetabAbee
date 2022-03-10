using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KetabAbee.Application.DTOs.Admin;
using KetabAbee.Application.DTOs.Admin.Contact;
using KetabAbee.Application.DTOs.Contact;
using KetabAbee.Domain.Models.ContactUs;

namespace KetabAbee.Application.Interfaces.Contact
{
    public interface IContactService
    {
        #region news letter

        Task<bool> AddEmailToNewsEmails(string email);

        bool EmailInNewsEmailsIsUnique(string email);

        IEnumerable<NewsLetter> GetNewsLetters();

        bool AddNewsLetter(CreateNewsLetterViewModel letter);

        bool SendNewsLetterToAll(int newsId);

        IEnumerable<NewsEmail> GetNewsLetterEmails();

        #endregion

        #region Contact us

        bool CreateContactUs(CreateContactUsViewModel contactUs, string userIp, int? userId);

        IEnumerable<ContactUs> GetContactUses();

        ContactUsesForAdminViewModel GetContactUsesForShowInAdmin(ContactUsesForAdminViewModel model);

        bool SendAnswerForContactUs(int contactId, string subject, string body);

        Task<bool> AddRequestBranch(CreateRequestBranchViewModel model);

        ShowBranchRequestsToAdminViewModel GetRequestsForShowAdmin(ShowBranchRequestsToAdminViewModel model);

        #endregion
    }
}
