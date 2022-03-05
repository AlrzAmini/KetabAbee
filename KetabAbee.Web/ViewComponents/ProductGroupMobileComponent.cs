using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KetabAbee.Application.Interfaces.Product;
using Microsoft.AspNetCore.Mvc;

namespace KetabAbee.Web.ViewComponents
{
    public class ProductGroupMobileComponent : ViewComponent
    {
        private readonly IProductService _productService;

        public ProductGroupMobileComponent(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View("ProductGroupMobile", await _productService.GetGroups());
        }
    }
}
