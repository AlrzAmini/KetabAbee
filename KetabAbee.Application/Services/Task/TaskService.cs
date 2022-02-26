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
using KetabAbee.Domain.Models.Task;
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

        public bool DeleteTask(int taskId)
        {
            try
            {
                var task = _taskRepository.GetTaskById(taskId);
                if (task == null)
                {
                    return false;
                }

                task.IsDelete = true;
                return _taskRepository.UpdateTask(task);
            }
            catch
            {
                return false;
            }
        }

        public FilterTasksViewModel FilterTasks(FilterTasksViewModel filter)
        {
            var result = GetTasksForManager().AsQueryable();

            #region filter role title

            if (!string.IsNullOrEmpty(filter.RoleSearch))
            {
                result = result.Where(r => r.RoleTitle.Contains(filter.RoleSearch));
            }

            #endregion

            #region filter date

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

            #endregion

            #region paging

            var pager = Pager.Build(filter.PageNum, result.Count(), filter.Take, filter.PageCountAfterAndBefor);
            var tasks = result.Paging(pager).ToList();

            #endregion

            return filter.SetPaging(pager).SetTasks(tasks);
        }

        public List<SelectListItem> GetRolesSelectList()
        {
            return _taskRepository.GetRoles()
                .Select(t => new SelectListItem
                {
                    Text = t.RoleTitle,
                    Value = t.RoleId.ToString()
                }).ToList();
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

        public TaskInfoViewModel GetTaskInfo(int taskId)
        {
            var task = _taskRepository.GetTaskByIdWithIncludes(taskId);
            if (task == null)
            {
                return new TaskInfoViewModel();
            }
            return new TaskInfoViewModel
            {
                CreateDate = task.CreateDate,
                RoleTitle = task.Role.RoleTitle,
                Body = task.Body,
                TaskPriority = task.TaskPriority,
                DeadLine = task.DeadLine,
                CreatorName = task.Creator.UserName,
                IsCompleted = task.IsCompleted,
                TaskId = task.TaskId
            };
        }

        public EditTaskViewModel GetTaskInfoForEdit(int taskId)
        {
            var task = _taskRepository.GetTaskById(taskId);
            if (task == null)
            {
                return new EditTaskViewModel();
            }

            return new EditTaskViewModel
            {
                UserId = task.CreatorId,
                CreateDate = task.CreateDate.ToShamsi(),
                Body = task.Body,
                TaskPriority = task.TaskPriority,
                DeadLine = task.DeadLine.ToShamsi(),
                RolesList = GetRolesSelectList(),
                SelectRoleId = task.RoleId,
                TaskId = task.TaskId
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
                TaskId = t.TaskId,
                IsCompleted = t.IsCompleted,
                TaskPriority = t.TaskPriority,
                Body = t.Body
            });
        }

        public EditTaskResult EditTask(EditTaskViewModel task)
        {
            try
            {
                if (task == null)
                {
                    return EditTaskResult.NotFound;
                }

                var createDate = task.CreateDate.ToMiladiDateTime();
                var deadLine = task.DeadLine.ToMiladiDateTime();
                if (createDate > deadLine)
                {
                    return EditTaskResult.DateError;
                }

                var newTask = new Domain.Models.Task.Task
                {
                    CreateDate = createDate,
                    DeadLine = deadLine,
                    RoleId = task.SelectRoleId,
                    Body = task.Body.Sanitizer(),
                    TaskPriority = task.TaskPriority,
                    CreatorId = task.UserId,
                    IsCompleted = task.IsCompleted,
                    TaskId = task.TaskId
                };

                return _taskRepository.UpdateTask(newTask) ? EditTaskResult.Success : EditTaskResult.Error;
            }
            catch
            {
                return EditTaskResult.Error;
            }
        }
    }
}
