using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
            return await Task.FromResult((IViewComponentResult)View("BestSellingBooks", _productService.GetBestSellingBooks().ToList()));
        }
    }
}
