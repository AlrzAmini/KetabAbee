using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KetabAbee.Application.DTOs.Paging;

namespace KetabAbee.Application.DTOs.Admin.User
{
    public class FilterUsersViewModel : BasePaging
    {
        #region Props

        public string UserName { get; set; }

        public string Email { get; set; }

        public string Mobile { get; set; }

        public FilterUserOrder OrderBy { get; set; }

        public List<Domain.Models.User.User> Users { get; set; }

        #endregion

        #region Methods

        public FilterUsersViewModel SetUsers(List<Domain.Models.User.User> users)
        {
            Users = users;
            return this;
        }

        public FilterUsersViewModel SetPaging(BasePaging paging)
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
    public enum FilterUserOrder
    {
        [Display(Name = "صعودی")]
        RegisterDateAsc,
        [Display(Name = "نزولی")]
        RegisterDateDsc

    }
}
