using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KetabAbee.Domain.Models.ContactUs;

namespace KetabAbee.Application.Interfaces.Contact
{
    public interface IContactService
    {
        bool AddEmailToNewsEmails(string email);

        bool EmailInNewsEmailsIsUnique(string email);
    }
}
