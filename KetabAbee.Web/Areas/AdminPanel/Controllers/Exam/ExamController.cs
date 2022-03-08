using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KetabAbee.Application.Const;
using KetabAbee.Application.Security;

namespace KetabAbee.Web.Areas.AdminPanel.Controllers.Exam
{
    [PermissionChecker(PerIds.AdminExams)]
    [Route("Admin/Exams")]
    public class ExamController : AdminBaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
