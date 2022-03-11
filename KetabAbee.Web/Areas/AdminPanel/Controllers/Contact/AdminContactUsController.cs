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
    public class AdminContactUsController : AdminBaseController
    {
        #region constructor

        private readonly IContactService _contactService;

        public AdminContactUsController(IContactService contactService)
        {
            _contactService = contactService;
        }

        #endregion

        #region index

        [HttpGet("Admin/Contact")]
        public IActionResult Index(ContactUsesForAdminViewModel model)
        {
            return View(_contactService.GetContactUsesForShowInAdmin(model));
        }

        #endregion

        #region answer to contact

        [HttpPost("Admin/Contact/Add/Answer"),ValidateAntiForgeryToken]
        public async Task<IActionResult> Answer(int contactId, string subject, string body)
        {
            if (string.IsNullOrEmpty(subject) || string.IsNullOrEmpty(contactId.ToString())) return BadRequest();
            if (!string.IsNullOrEmpty(body))
            {
                if (await _contactService.SendAnswerForContactUs(contactId,subject,body))
                {
                    TempData["SuccessMessage"] = "پاسخ با موفقیت ارسال شد";
                    return RedirectToAction("Index");
                }
                TempData["ErrorMessage"] = "خطایی در ارسال پاسخ رخ داد";
                return RedirectToAction("Index");
            }
            TempData["ErrorMessage"] = "فیلد متن پیغام را پر کنید";
            return RedirectToAction("Index");

        }

        #endregion
    }
}
