using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KetabAbee.Application.Const;
using KetabAbee.Application.DTOs.Admin.Products.Book.Publishers;
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

        #region edit publisher

        [HttpGet("Edit/{publisherId}")]
        public IActionResult EditPublisher(int publisherId)
        {
            var publisher = _productService.GetPublisherInfoForEdit(publisherId);

            if (publisher != null) return View(publisher);

            TempData["WarningMessage"] = "ناشر مورد نظر یافت نشد";
            return RedirectToAction("Index");
        }

        [HttpPost("Edit/{publisherId}"), ValidateAntiForgeryToken]
        public IActionResult EditPublisher(EditPublisherViewModel publisher)
        {
            if (!ModelState.IsValid)
            {
                return View(publisher);
            }

            var res = _productService.EditPublisher(publisher);

            switch (res)
            {
                case EditPublisherResult.Success:
                    TempData["SuccessMessage"] = "ناشر با موفقیت ویرایش شد";
                    return RedirectToAction("Index");
                case EditPublisherResult.RepetitiousName:
                    TempData["WarningMessage"] = "نام ناشر تکراری است";
                    return RedirectToAction("EditPublisher", new { publisher });
                case EditPublisherResult.Error:
                    TempData["ErrorMessage"] = "ناشر ویرایش نشد";
                    return RedirectToAction("EditPublisher", new { publisher });
                default:
                    TempData["ErrorMessage"] = "ناشر ویرایش نشد";
                    return RedirectToAction("EditPublisher", new { publisher });
            }
        }

        #endregion

        #region add publisher

        [HttpGet("P/Add")]
        public IActionResult AddPublisher()
        {
            return View();
        }

        [HttpPost("P/Add"), ValidateAntiForgeryToken]
        public IActionResult AddPublisher(CreatePublisherViewModel publisher)
        {
            if (!ModelState.IsValid)
            {
                return View(publisher);
            }

            var res = _productService.AddPublisherFromAdmin(publisher);

            switch (res)
            {
                case CreatePublisherResult.Success:
                    TempData["SuccessMessage"] = "ناشر افزوده شد";
                    return RedirectToAction("Index");
                case CreatePublisherResult.RepetitiousName:
                    TempData["WarningMessage"] = "نام ناشر تکراری است";
                    return RedirectToAction("AddPublisher", new { publisher });
                case CreatePublisherResult.Error:
                    TempData["ErrorMessage"] = "ناشر افزوده نشد";
                    return RedirectToAction("AddPublisher", new { publisher });
                default:
                    TempData["ErrorMessage"] = "ناشر افزوده نشد";
                    return RedirectToAction("AddPublisher", new { publisher });
            }
        }

        #endregion
    }
}
