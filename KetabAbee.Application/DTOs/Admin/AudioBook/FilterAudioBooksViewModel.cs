using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KetabAbee.Application.DTOs.Paging;

namespace KetabAbee.Application.DTOs.Admin.AudioBook
{
    public class FilterAudioBooksViewModel : BasePaging
    {
        #region properties

        public string Title { get; set; }

        public string Writer { get; set; }

        public string Speaker { get; set; }

        public float MinFileSize { get; set; }
        public float MaxFileSize { get; set; }

        public List<ShowAudioBookInListViewModel> AudioBooks { get; set; }

        #endregion

        #region Methods

        public FilterAudioBooksViewModel SetAudioBooks(List<ShowAudioBookInListViewModel> audioBooks)
        {
            AudioBooks = audioBooks;
            return this;
        }

        public FilterAudioBooksViewModel SetPaging(BasePaging paging)
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
