using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KetabAbee.Application.Extensions;
using KetabAbee.Application.Interfaces.Product;
using Microsoft.AspNetCore.Mvc;

namespace KetabAbee.Web.ViewComponents
{
    public class BestSellingBooksComponent : ViewComponent
    {
        private readonly IProductService _productService;

        public BestSellingBooksComponent(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            if (User.Identity.IsAuthenticated)
            {
                return await Task.FromResult((IViewComponentResult)View("BestSellingBooks", _productService.GetBestSellingBooks(HttpContext.User.GetUserId()).ToList()));
            }
            return await Task.FromResult((IViewComponentResult)View("BestSellingBooks", _productService.GetBestSellingBooks(null).ToList()));
        }
    }
}
