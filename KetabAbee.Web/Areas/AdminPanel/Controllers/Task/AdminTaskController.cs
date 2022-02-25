using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KetabAbee.Application.Const;
using KetabAbee.Application.Security;

namespace KetabAbee.Web.Areas.AdminPanel.Controllers.Task
{
    [RoleChecker(RoleIds.Manager)]
    [Route("Admin/Tasks")]
    public class AdminTaskController : AdminBaseController
    {
        #region constructor

        

        #endregion

        #region index

        public IActionResult Index()
        {
            return View();
        }

        #endregion

        #region Create Task

        [HttpGet("Add")]
        public IActionResult CreateTask()
        {
            return View();
        }

        #endregion
    }
}
