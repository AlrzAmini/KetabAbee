using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KetabAbee.Application.DTOs.Admin.Order;
using KetabAbee.Application.DTOs.Paging;
using KetabAbee.Domain.Models.ContactUs;

namespace KetabAbee.Application.DTOs.Admin.Contact
{
    public class ContactUsesForAdminViewModel : BasePaging
    {
        public List<ContactUs> ContactUsList { get; set; }

        #region Methods

        public ContactUsesForAdminViewModel SetContactUses(List<ContactUs> contactUsList)
        {
            ContactUsList = contactUsList;
            return this;
        }

        public ContactUsesForAdminViewModel SetPaging(BasePaging paging)
        {
            PageNum = paging.PageNum;
            TotalEntitiesCount = paging.TotalEntitiesCount;
            StartPage = paging.StartPage;
            EndPage = paging.EndPage;
            PageCountAfterAndBefor = paging.PageCountAfterAndBefor;
            Take = paging.Take;
            Skip = paging.Skip;
            TotalPages = paging.TotalPages;

            return this;
        }

        #endregion
    }
}
