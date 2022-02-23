
using System.Collections.Generic;
using KetabAbee.Application.DTOs.Paging;

namespace KetabAbee.Application.DTOs.Admin.Comment
{
    public class FilterCommentsViewModel : BasePaging
    {
        #region props

        public string UserName { get; set; }

        public string Body { get; set; }

        public bool IsNotReadByAdmin { get; set; }

        public List<CommentInAdminViewModel> Comments { get; set; }

        #endregion

        #region Methods

        public FilterCommentsViewModel SetComments(List<CommentInAdminViewModel> comments)
        {
            Comments = comments;
            return this;
        }

        public FilterCommentsViewModel SetPaging(BasePaging paging)
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
