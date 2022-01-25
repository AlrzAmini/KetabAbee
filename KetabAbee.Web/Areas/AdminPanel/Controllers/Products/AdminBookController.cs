using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KetabAbee.Application.Const;
using KetabAbee.Application.DTOs.Admin.Products.Book;
using KetabAbee.Application.Interfaces.Product;
using KetabAbee.Application.Security;
using KetabAbee.Domain.Models.Products;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace KetabAbee.Web.Areas.AdminPanel.Controllers.Products
{
    [PermissionChecker(PerIds.Admin‌Books)]
    [Route("Admin/Books")]
    public class AdminBookController : AdminBaseController
    {
        #region constructor

        private readonly IProductService _productService;

        public AdminBookController(IProductService productService)
        {
            _productService = productService;
        }

        #endregion

        #region Book List

        [HttpGet]
        public IActionResult Index(FilterBooksViewModel filter)
        {
            return View(_productService.GetBooksByFilter(filter));
        }

        #endregion

        #region Add Book

        [HttpGet("AddBook")]
        public IActionResult AddBook()
        {
            #region Publishers

            // its not the best way to do it but i just want to show this way
            var listPublishers = new List<SelectListItem>
            {
                new(){Text = "ناشر را انتخاب کنید",Value = ""}
            };
            var publishers = _productService.GetPublishers()
                .Select(p => new SelectListItem
                {
                    Text = p.PublisherName,
                    Value = p.PublisherId.ToString()
                }).ToList();
            listPublishers.AddRange(publishers);
            ViewBag.Publishers = listPublishers;

            #endregion

            #region Main Group

            var groups = _productService.GetGroupsForAddBook();
            var selectGroups = new List<SelectListItem>
            {
                new(){Text = "دسته بندی اصلی را انتخاب کنید",Value = ""}
            };
            selectGroups.AddRange(groups);
            ViewBag.Groups = selectGroups;

            #endregion

            #region Sub 1

            var subGroups = _productService.GetSubGroupsForAddBook(int.Parse(groups.First().Value));
            var selectSubGroups = new List<SelectListItem>
            {
                new(){Text = "دسته بندی فرعی اول را انتخاب کنید",Value = ""}
            };
            selectSubGroups.AddRange(subGroups);
            ViewBag.SubGroups = selectSubGroups;

            #endregion

            #region Sub 2

            var sub2Groups = _productService.GetSubGroupsForAddBook(int.Parse(subGroups.First().Value));
            var selectSub2Groups = new List<SelectListItem>
            {
                new(){Text = "دسته بندی فرعی دوم را انتخاب کنید",Value = ""}
            };
            selectSub2Groups.AddRange(sub2Groups);
            ViewBag.Sub2Groups = selectSub2Groups;

            #endregion

            return View();
        }

        [HttpPost("AddBook")]
        public IActionResult AddBook(Book book, IFormFile imgFile)
        {
            if (!ModelState.IsValid)
            {
                return View(book);
            }
            if (_productService.AddBook(book, imgFile))
            {
                TempData["SuccessMessage"] = "ثبت کتاب با موفقیت انجام شد";
                return RedirectToAction("Index");
            }

            TempData["ErrorMessage"] = "در هنگام ثبت کتاب مشکلی بوجود آمد";
            return RedirectToAction("Index");
        }

        #endregion

        #region delete Book

        [HttpGet("DeleteBook/{bookId}")]
        public IActionResult DeleteBook(int bookId)
        {
            if (_productService.DeleteBook(bookId))
            {
                TempData["SuccessMessage"] = "حذف کتاب با موفقیت انجام شد";
                return RedirectToAction("Index");
            }
            TempData["ErrorMessage"] = "در هنگام حذف کتاب مشکلی بوجود آمد";
            return RedirectToAction("Index");
        }

        #endregion

        #region Add Publisher

        [Route("AddPublisher")]
        public IActionResult AddPublisher(Publisher publisher)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("AddBook");
            }

            if (_productService.IsPublisherUnique(publisher.PublisherName))
            {
                TempData["ErrorMessage"] = "ناشر وارد شده در لیست شما موجود است";
                return RedirectToAction("AddBook");
            }

            if (_productService.AddPublisher(publisher))
            {
                TempData["SuccessMessage"] = "ناشر افزوده شد";
                return RedirectToAction("AddBook");
            }

            TempData["ErrorMessage"] = "افزودن ناشر با شکست مواجه شد";
            return RedirectToAction("AddBook");
        }

        #endregion

        #region Edit book

        [HttpGet("EditBook/{bookId}")]
        public IActionResult EditBook(int bookId)
        {
            var book = _productService.GetBookById(bookId);

            #region Main Group

            var groups = _productService.GetGroupsForSelect();
            ViewBag.Groups = new SelectList(groups, "Value", "Text", book.GroupId);

            #endregion

            #region Sub 1

            var sub1Groups = _productService.GetSubGroupsForAddBook(book.GroupId);
            ViewBag.sub1Groups = new SelectList(sub1Groups, "Value", "Text", book.SubGroupId);

            #endregion

            #region Sub 2

            var sub2Groups = _productService.GetSubGroupsForAddBook(book.SubGroupId);
            ViewBag.sub2Groups = new SelectList(sub2Groups, "Value", "Text", book.SubGroup2Id);

            #endregion

            #region Publishers

            
            var publishers = _productService.GetPublishersForSelect();
            ViewBag.publishers = new SelectList(publishers, "Value", "Text", book.PublisherId);

            #endregion

            return View(book);
        }

        [HttpPost("EditBook/{bookId}")]
        public IActionResult EditBook(Book book,IFormFile imgFile)
        {
            if (!ModelState.IsValid)
            {
                return View(book);
            }

            if (_productService.EditBook(book,imgFile))
            {
                TempData["SuccessMessage"] = "ویرایش کتاب با موفقیت انجام شد";
                return RedirectToAction("Index");
            }
            
            TempData["ErrorMessage"] = "ویرایش کتاب با شکست مواجه شد";
            return RedirectToAction("Index");
        }

        #endregion

        #region Change inventory

        [HttpGet("ChangeInventory/{bookId}")]
        public IActionResult ChangeInventory(int bookId)
        {
            ViewBag.BookInventoryList = _productService.GetBookChangeInventoryReports(bookId).ToList();
            return View(_productService.GetInventoryInfoByBookId(bookId));
        }

        [HttpPost("ChangeInventory/{bookId}")]
        public IActionResult ChangeInventory(ChangeInventoryViewModel inventory)
        {
            if (!ModelState.IsValid)
            {
                return View(inventory);
            }

            if (!string.IsNullOrEmpty(inventory.IncNumber.ToString()) && !string.IsNullOrEmpty(inventory.DecNumber.ToString()))
            {
                TempData["ErrorMessage"] = "هر دو فیلد افزایش و کاهش موجودی نمی توانند همزمان اعمال شوند";
                inventory = _productService.GetInventoryInfoByBookId(inventory.BookId);
                return View(inventory);
            }

            if (!string.IsNullOrEmpty(inventory.IncNumber.ToString()))
            {
                if (inventory.IncNumber < 0)
                {
                    TempData["ErrorMessage"] = "مقدار  افزایش موجودی نا معتبر است";
                    inventory = _productService.GetInventoryInfoByBookId(inventory.BookId);
                    return View(inventory);
                }

                if (_productService.IncreaseInventory(inventory))
                {
                    TempData["SuccessMessage"] = "افزایش موجودی با موفقیت انجام شد";
                    return RedirectToAction("Index");
                }
                TempData["ErrorMessage"] = "افزایش موجودی با شکست مواجه شد";
                return RedirectToAction("Index");
            }

            if (!string.IsNullOrEmpty(inventory.DecNumber.ToString()))
            {
                if (inventory.DecNumber < 0)
                {
                    TempData["ErrorMessage"] = "مقدار  کاهش موجودی نا معتبر است";
                    inventory = _productService.GetInventoryInfoByBookId(inventory.BookId);
                    return View(inventory);
                }

                var res = _productService.DecreaseInventory(inventory);
                switch (res)
                {
                    case "Success":
                        TempData["SuccessMessage"] = "کاهش موجودی با موفقیت انجام شد";
                        return RedirectToAction("Index");
                    case "NegativeError":
                        TempData["WarningMessage"] = "موجودی کتاب نمی تواند تبدیل به عددی منفی شود";
                        inventory = _productService.GetInventoryInfoByBookId(inventory.BookId);
                        return View(inventory);
                    default:
                        TempData["ErrorMessage"] = "کاهش موجودی با شکست مواجه شد";
                        return RedirectToAction("Index");
                }
            }

            return RedirectToAction("Index");
        }

        #endregion

        #region change inventory list

        [HttpGet("AllInventoryReports")]
        public IActionResult AllInventoryReports()
        {
            return View(_productService.GetAllInventoryReports().ToList());
        }

        #endregion
    }
}
