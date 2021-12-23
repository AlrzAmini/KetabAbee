using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KetabAbee.Web.Areas.AdminPanel.Controllers
{
    public class AdminHomeController : AdminBaseController
    {
        #region Index

        [HttpGet("Admin")]
        public IActionResult Home()
        {
            return View();
        }

        #endregion

    }
}
