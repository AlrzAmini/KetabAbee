using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KetabAbee.Application.Const;
using KetabAbee.Application.Security;
using Microsoft.AspNetCore.Authorization;

namespace KetabAbee.Web.Areas.AdminPanel.Controllers
{
    [PermissionChecker(PerIds.AdminPanel)]
    [Area("AdminPanel")]
    public class AdminBaseController : Controller
    {
    }
}
