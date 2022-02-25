using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KetabAbee.Application.Convertors;
using KetabAbee.Application.DTOs.Admin.Task;
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
    }
}
