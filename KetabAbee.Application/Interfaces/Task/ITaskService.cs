using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KetabAbee.Application.DTOs.Admin.Task;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace KetabAbee.Application.Interfaces.Task
{
    public interface ITaskService
    {
        CreateTaskViewModel GetRolesForCreateTask();

        CreateTaskResult AddTask(CreateTaskViewModel task);

        IEnumerable<ShowTaskInListForManager> GetTasksForManager();

        FilterTasksViewModel FilterTasks(FilterTasksViewModel filter);

        bool DeleteTask(int taskId);

        TaskInfoViewModel GetTaskInfo(int taskId);

        EditTaskViewModel GetTaskInfoForEdit(int taskId);

        List<SelectListItem> GetRolesSelectList();

        EditTaskResult EditTask(EditTaskViewModel task);
    }
}
