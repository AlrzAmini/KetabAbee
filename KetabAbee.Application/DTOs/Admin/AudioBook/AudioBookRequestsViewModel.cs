using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KetabAbee.Application.DTOs.Paging;
using KetabAbee.Application.DTOs.Ticket;

namespace KetabAbee.Application.DTOs.Admin.AudioBook
{
    public class AudioBookRequestsViewModel : BasePaging
    {
        #region props

        public List<ShowAudioBookRequestInList> Requests { get; set; }

        #endregion

        #region Methods

        public AudioBookRequestsViewModel SetRequests(List<ShowAudioBookRequestInList> requests)
        {
            Requests = requests;
            return this;
        }

        public AudioBookRequestsViewModel SetPaging(BasePaging paging)
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
