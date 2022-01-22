using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
            return await Task.FromResult((IViewComponentResult)View("LatestBooks", _productService.GetLatestBooksInIndex(10).ToList()));
        }
    }
}
