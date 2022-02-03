using KetabAbee.Domain.Models.ContactUs;

namespace KetabAbee.Domain.Interfaces
{
    public interface IContactRepository
    {
        #region newsletteremails

        bool AddEmailToNewsEmails(NewsEmail email);

        bool EmailIsUnique(string email);

        #endregion
    }
}
