using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace KetabAbee.Web.Areas.AdminPanel.Controllers
{
    [Authorize]
    [Area("AdminPanel")]
    public class AdminBaseController : Controller
    {
    }
}
