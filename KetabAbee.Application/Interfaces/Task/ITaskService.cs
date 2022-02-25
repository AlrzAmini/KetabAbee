using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KetabAbee.Application.DTOs.Admin.Task;

namespace KetabAbee.Application.Interfaces.Task
{
    public interface ITaskService
    {
        CreateTaskViewModel GetRolesForCreateTask();

        CreateTaskResult AddTask(CreateTaskViewModel task);

        IEnumerable<ShowTaskInListForManager> GetTasksForManager();

        FilterTasksViewModel FilterTasks(FilterTasksViewModel filter);
    }
}
