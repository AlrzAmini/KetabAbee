using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KetabAbee.Application.Const;
using KetabAbee.Application.DTOs.Admin.Task;
using KetabAbee.Application.Extensions;
using KetabAbee.Application.Interfaces.Task;
using KetabAbee.Application.Security;

namespace KetabAbee.Web.Areas.AdminPanel.Controllers.Task
{
    [RoleChecker(RoleIds.Manager)]
    [Route("Admin/Tasks")]
    public class AdminTaskController : AdminBaseController
    {
        #region constructor

        private readonly ITaskService _taskService;

        public AdminTaskController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        #endregion

        #region index

        public IActionResult Index(FilterTasksViewModel filter)
        {
            return View(_taskService.FilterTasks(filter));
        }

        #endregion

        #region Create Task

        [HttpGet("Add")]
        public IActionResult CreateTask()
        {
            return View(_taskService.GetRolesForCreateTask());
        }

        [HttpPost("Add"), ValidateAntiForgeryToken]
        public IActionResult CreateTask(CreateTaskViewModel task)
        {
            if (!ModelState.IsValid)
            {
                task.RolesList = _taskService.GetRolesForCreateTask().RolesList;
                return View(task);
            }

            task.UserId = User.GetUserId();

            var res = _taskService.AddTask(task);
            switch (res)
            {
                case CreateTaskResult.Success:
                    TempData["SuccessMessage"] = "تسک با موفقیت افزوده شد";
                    return RedirectToAction("Index");
                case CreateTaskResult.Error:
                    TempData["ErrorMessage"] = "تسک افزوده نشد";
                    return RedirectToAction("CreateTask");
                case CreateTaskResult.DateError:
                    TempData["WarningMessage"] = "تاریخ شروع نمیتواند بعد از تاریخ پایان باشد";
                    return RedirectToAction("CreateTask");
                default:
                    TempData["ErrorMessage"] = "مشکلی در انجام عملیات رخ داد";
                    return RedirectToAction("Index");
            }
        }

        #endregion

        #region delete task

        [HttpGet("Delete/{taskId}")]
        public IActionResult DeleteTask(int taskId)
        {
            if (_taskService.DeleteTask(taskId))
            {
                TempData["SuccessMessage"] = "تسک با موفقیت حذف شد";
                return RedirectToAction("Index");
            }
            TempData["ErrorMessage"] = "تسک حذف نشد";
            return RedirectToAction("Index");
        }

        #endregion
    }
}
