using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KetabAbee.Application.Const;
using KetabAbee.Application.Interfaces.Product;
using KetabAbee.Application.Security;
using KetabAbee.Domain.Models.Products;

namespace KetabAbee.Web.Areas.AdminPanel.Controllers.Products
{
    [PermissionChecker(PerIds.AdminGroups)]
    [Route("Admin/Groups")]
    public class ProductGroupController : AdminBaseController
    {
        #region constructor

        private readonly IProductService _productService;

        public ProductGroupController(IProductService productService)
        {
            _productService = productService;
        }

        #endregion

        #region List Groups

        public IActionResult Index()
        {
            return View(_productService.GetGroupsForAdmin().ToList());
        }

        #endregion

        #region Add Group

        [HttpGet("AddGroup")]
        public IActionResult AddGroup(int? id) // id = groupId
        {
            var group = new ProductGroup
            {
                ParentId = id
            };
            return View(group);
        }

        [HttpPost("AddGroup")]
        public IActionResult AddGroup(ProductGroup group) // id = groupId
        {
            if (!ModelState.IsValid)
            {
                return View(group);
            }

            if (_productService.AddGroup(group))
            {
                TempData["SuccessMessage"] = "ثبت دسته بندی با موفقیت انجام شد";
                return RedirectToAction("Index");
            }

            TempData["ErrorMessage"] = "ثبت دسته بندی با شکست مواجه شد";
            return RedirectToAction("Index");
        }

        #endregion

    }
}
