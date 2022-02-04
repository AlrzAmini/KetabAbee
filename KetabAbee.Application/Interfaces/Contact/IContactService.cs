using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KetabAbee.Application.DTOs.Contact;
using KetabAbee.Domain.Models.ContactUs;

namespace KetabAbee.Application.Interfaces.Contact
{
    public interface IContactService
    {
        #region news email

        bool AddEmailToNewsEmails(string email);

        bool EmailInNewsEmailsIsUnique(string email);

        #endregion

        #region Contact us

        bool CreateContactUs(CreateContactUsViewModel contactUs, string userIp, int? userId);

        #endregion
    }
}
