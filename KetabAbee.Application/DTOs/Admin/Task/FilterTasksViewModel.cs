using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KetabAbee.Application.DTOs.Paging;
using KetabAbee.Domain.Models.Task;

namespace KetabAbee.Application.DTOs.Admin.Task
{
    public class FilterTasksViewModel : BasePaging
    {
        #region properties

        public string RoleSearch { get; set; }

        public string StartDateSearch { get; set; }

        public string EndDateSearch { get; set; }

        public List<ShowTaskInListForManager> Tasks { get; set; }

        #endregion

        #region Methods

        public FilterTasksViewModel SetTasks(List<ShowTaskInListForManager> tasks)
        {
            Tasks = tasks;
            return this;
        }

        public FilterTasksViewModel SetPaging(BasePaging paging)
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
