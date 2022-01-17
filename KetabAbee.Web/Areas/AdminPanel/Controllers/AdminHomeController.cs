using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KetabAbee.Application.Interfaces.Product;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace KetabAbee.Web.Areas.AdminPanel.Controllers
{
    public class AdminHomeController : AdminBaseController
    {
        private readonly IProductService _productService;

        public AdminHomeController(IProductService productService)
        {
            _productService = productService;
        }

        #region Index

        [HttpGet("Admin")]
        public IActionResult Home()
        {
            return View();
        }

        #endregion

        public IActionResult GetSubGroups(int id) // id = groupId
        {
            var list = new List<SelectListItem>
            {
                new(){Text = "دسته بندی فرعی اول را انتخاب کنید",Value = ""}
            };
            list.AddRange(_productService.GetSubGroupsForAddBook(id));
            return Json(new SelectList(list, "Value", "Text"));
        }

        public IActionResult GetSubGroups2(int id) // id = sub id 1
        {
            var list = new List<SelectListItem>
            {
                new(){Text = "دسته بندی فرعی دوم را انتخاب کنید",Value = ""}
            };
            list.AddRange(_productService.GetSubGroupsForAddBook(id));
            return Json(new SelectList(list, "Value", "Text"));
        }

    }
}
