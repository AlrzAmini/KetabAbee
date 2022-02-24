using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KetabAbee.Application.Const;
using KetabAbee.Application.Interfaces.Product;
using KetabAbee.Application.Security;

namespace KetabAbee.Web.Areas.AdminPanel.Controllers.Products
{
    [PermissionChecker(PerIds.AdminBooks)]
    [Route("Admin/Publishers")]
    public class AdminPublisherController : AdminBaseController
    {
        #region constructor

        private readonly IProductService _productService;

        public AdminPublisherController(IProductService productService)
        {
            _productService = productService;
        }

        #endregion

        #region index

        [HttpGet]
        public IActionResult Index()
        {
            return View(_productService.GetPublishersForAdmin().ToList());
        }

        #endregion

        #region Publisher Books

        [HttpGet("P/{publisherId}/Books")]
        public IActionResult PublisherBooks(int publisherId)
        {
            var model = _productService.GetPublisherBooks(publisherId).ToList();
            if (model.Any()) return View(model);

            TempData["InfoMessage"] = "این ناشر کتابی ندارد";
            return RedirectToAction("Index");
        }

        #endregion

        #region publisher books count

        [HttpGet("Get/publisher-books-count/json")]
        public IActionResult GetPublisherBooksCount(int publisherId)
        {
            var data = _productService.PublisherBooksCount(publisherId);
            return new JsonResult(data);
        }

        #endregion

        #region Delete Publisher

        [HttpGet("P/Delete/{publisherId}")]
        public IActionResult DeletePublisher(int publisherId)
        {
            if (_productService.DeletePublisher(publisherId))
            {
                TempData["SuccessMessage"] = "ناشر با موفقیت حذف شد";
                return RedirectToAction("Index");
            }

            TempData["ErrorMessage"] = "مشکلی در حذف ناشر رخ داد";
            return RedirectToAction("Index");
        }

        #endregion

    }
}
