using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KetabAbee.Application.Extensions;
using KetabAbee.Application.Interfaces.Product;
using Microsoft.AspNetCore.Mvc;

namespace KetabAbee.Web.ViewComponents
{
    public class LatestBooksComponent : ViewComponent
    {
        private readonly IProductService _productService;

        public LatestBooksComponent(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            if (User.Identity.IsAuthenticated)
            {
                return View("LatestBooks",
                    await _productService.GetLatestBooksInIndex(10, HttpContext.User.GetUserId()));
            }
            return View("LatestBooks",
                await _productService.GetLatestBooksInIndex(10, null));
        }
    }
}
