using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KetabAbee.Application.Interfaces.Product;
using KetabAbee.Domain.Models.Products;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace KetabAbee.Web.Areas.AdminPanel.Controllers.Products
{
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

        public IActionResult Index()
        {
            return View();
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
        public IActionResult AddBook(Book book)
        {
            return View();
        }

        #endregion
    }
}
