using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KetabAbee.Application.Convertors;
using KetabAbee.Application.DTOs.Admin.Task;
using KetabAbee.Application.DTOs.Paging;
using KetabAbee.Application.Extensions;
using KetabAbee.Application.Interfaces.Task;
using KetabAbee.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace KetabAbee.Application.Services.Task
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;

        public TaskService(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public CreateTaskResult AddTask(CreateTaskViewModel task)
        {
            try
            {
                var createDate = task.CreateDate.ToMiladiDateTime();
                var deadLine = task.DeadLine.ToMiladiDateTime();

                if (createDate > deadLine)
                {
                    return CreateTaskResult.DateError;
                }

                var newTask = new Domain.Models.Task.Task
                {
                    Body = task.Body.Sanitizer(),
                    CreateDate = createDate,
                    DeadLine = deadLine,
                    RoleId = task.SelectRoleId,
                    TaskPriority = task.TaskPriority,
                    CreatorId = task.UserId
                };

                return _taskRepository.AddTask(newTask) ? CreateTaskResult.Success : CreateTaskResult.Error;
            }
            catch
            {
                return CreateTaskResult.Error;
            }
        }

        public FilterTasksViewModel FilterTasks(FilterTasksViewModel filter)
        {
            var result = GetTasksForManager().AsQueryable();

            if (!string.IsNullOrEmpty(filter.RoleSearch))
            {
                result = result.Where(r => r.RoleTitle.Contains(filter.RoleSearch));
            }

            if (!string.IsNullOrEmpty(filter.StartDateSearch))
            {
                var sDate = filter.StartDateSearch.ToMiladiDateTime();
                result = result.Where(r => r.CreateDate > sDate);
            }
            if (!string.IsNullOrEmpty(filter.EndDateSearch))
            {
                var eDate = filter.EndDateSearch.ToMiladiDateTime();
                result = result.Where(r => r.DeadLine < eDate);
            }

            var pager = Pager.Build(filter.PageNum, result.Count(), filter.Take, filter.PageCountAfterAndBefor);
            var tasks = result.Paging(pager).ToList();

            return filter.SetPaging(pager).SetTasks(tasks);
        }

        public CreateTaskViewModel GetRolesForCreateTask()
        {
            return new CreateTaskViewModel
            {
                RolesList = _taskRepository.GetRoles()
                    .Select(r => new ShowRolesInCreateTaskViewModel
                    {
                        RoleId = r.RoleId,
                        RoleTitle = r.RoleTitle
                    }).Select(r => new SelectListItem
                    {
                        Text = r.RoleTitle,
                        Value = r.RoleId.ToString()
                    }).ToList()
            };
        }

        public IEnumerable<ShowTaskInListForManager> GetTasksForManager()
        {
            return _taskRepository.GetTasks().Select(t => new ShowTaskInListForManager
            {
                CreateDate = t.CreateDate,
                DeadLine = t.DeadLine,
                RoleTitle = t.Role.RoleTitle,
                CreatorName = t.Creator.UserName,
                TaskId = t.TaskId
            });
        }
    }
}
