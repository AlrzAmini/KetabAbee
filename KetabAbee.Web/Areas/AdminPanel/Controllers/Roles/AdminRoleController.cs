using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KetabAbee.Web.Areas.AdminPanel.Controllers.Roles
{
    public class AdminRoleController : AdminBaseController
    {
        #region List Roles

        public IActionResult Index()
        {
            return View();
        }

        #endregion
    }
}
