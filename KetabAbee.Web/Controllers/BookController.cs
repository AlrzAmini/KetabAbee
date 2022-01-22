using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KetabAbee.Application.DTOs.Book;
using KetabAbee.Application.Interfaces.Product;
using KetabAbee.Domain.Models.Products;

namespace KetabAbee.Web.Controllers
{
    public class BookController : Controller
    {
        #region construcor

        private readonly IProductService _productService;

        public BookController(IProductService productService)
        {
            _productService = productService;
        }

        #endregion

        #region Books & Filter

        [HttpGet("Book")]
        public IActionResult Index(FilterBookListViewModel filter)
        {
            ViewBag.Groups = _productService.GetGroups().ToList();
            ViewBag.Publishers = _productService.GetPublishers().ToList();

            if (filter.Exist)
            {
                filter.ExistStatus = ExistStatus.Exist;
            }

            return View(_productService.GetBooksForIndex(filter));
        }

        #endregion

        #region Book Info

        [HttpGet("BookInfo/{bookId}")]
        public IActionResult BookInfo(int bookId)
        {
            var model = _productService.GetBookForShowByBookId(bookId);

            ViewBag.PublisherBooks = _productService.PublisherBooks(model.PublisherId).ToList();

            return View(model);
        }

        #endregion

    }
}
