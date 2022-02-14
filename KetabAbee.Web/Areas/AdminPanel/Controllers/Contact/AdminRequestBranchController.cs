using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KetabAbee.Application.Const;
using KetabAbee.Application.DTOs.Admin.Contact;
using KetabAbee.Application.Interfaces.Contact;
using KetabAbee.Application.Security;

namespace KetabAbee.Web.Areas.AdminPanel.Controllers.Contact
{
    [PermissionChecker(PerIds.AdminContactUses)]
    public class AdminRequestBranchController : AdminBaseController
    {
        #region ctorf

        private readonly IContactService _contactService;

        public AdminRequestBranchController(IContactService contactService)
        {
            _contactService = contactService;
        }

        #endregion

        #region index

        public IActionResult Index(ShowBranchRequestsToAdminViewModel model)
        {
            return View(_contactService.GetRequestsForShowAdmin(model));
        }

        #endregion
    }
}
