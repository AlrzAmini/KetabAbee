using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KetabAbee.Web.Controllers
{
    public class AccountController : Controller
    {
        #region Register

        [Route("Register")]
        public IActionResult Register()
        {
            return View();
        }

        #endregion

    }
}
