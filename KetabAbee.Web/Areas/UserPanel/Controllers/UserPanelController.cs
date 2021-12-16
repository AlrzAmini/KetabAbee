using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KetabAbee.Web.Areas.UserPanel.Controllers
{
    public class UserPanelController : UserPanelBaseController
    {
        #region constructor



        #endregion

        #region Dashborad

        [HttpGet("UserPanel/Dashboard")]
        public async Task<IActionResult> Dashboard()
        {
            return View();
        }

        #endregion

    }
}
