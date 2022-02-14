using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KetabAbee.Application.DTOs.Paging;
using KetabAbee.Domain.Models.ContactUs;

namespace KetabAbee.Application.DTOs.Admin.Contact
{
    public class ShowBranchRequestsToAdminViewModel : BasePaging
    {
        public List<RequestBranch> Requests { get; set; }

        #region Methods

        public ShowBranchRequestsToAdminViewModel SetRequests(List<RequestBranch> requests)
        {
            Requests = requests;
            return this;
        }

        public ShowBranchRequestsToAdminViewModel SetPaging(BasePaging paging)
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
