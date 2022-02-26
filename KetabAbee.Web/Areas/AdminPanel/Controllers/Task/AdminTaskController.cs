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

        #region task info

        [HttpGet("Info/{taskId}")]
        public IActionResult TaskInfo(int taskId)
        {
            return View(_taskService.GetTaskInfo(taskId));
        }

        #endregion

        #region edit task

        [HttpGet("Edit/{taskId}")]
        public IActionResult EditTask(int taskId)
        {
            return View(_taskService.GetTaskInfoForEdit(taskId));
        }

        [HttpPost("Edit/{taskId}"), ValidateAntiForgeryToken]
        public IActionResult EditTask(EditTaskViewModel edit)
        {
            if (!ModelState.IsValid)
            {
                return View(edit);
            }

            var res = _taskService.EditTask(edit);
            switch (res)
            {
                case EditTaskResult.Success:
                    TempData["SuccessMessage"] = "با موفقیت ویرایش شد";
                    return RedirectToAction("Index");
                case EditTaskResult.NotFound:
                    TempData["WarningMessage"] = "تسکی یافت نشد";
                    return RedirectToAction("EditTask", new { taskId = edit.TaskId });
                case EditTaskResult.DateError:
                    TempData["WarningMessage"] = "تاریخ پایان نمیتواند کوچکتر از تاریخ شروع باشد";
                    return RedirectToAction("EditTask", new { taskId = edit.TaskId });
                case EditTaskResult.Error:
                    TempData["ErrorMessage"] = "خطایی در انجام عملیات رخ داد";
                    return RedirectToAction("EditTask", new { taskId = edit.TaskId });
                default:
                    TempData["ErrorMessage"] = "خطایی در انجام عملیات رخ داد";
                    return RedirectToAction("Index");
            }
        }

        #endregion
    }
}
