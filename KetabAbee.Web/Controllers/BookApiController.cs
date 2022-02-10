using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KetabAbee.Application.Interfaces.Product;

namespace KetabAbee.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookApiController : ControllerBase
    {
        #region constructor

        private readonly IProductService _productService;

        public BookApiController(IProductService productService)
        {
            _productService = productService;
        }

        #endregion

        #region search auto complete book name

        [Produces("application/json")]
        [HttpGet("search-autocomplete-bookName")]
        public IActionResult SearchBookName()
        {
            var term = HttpContext.Request.Query["term"].ToString();
            var bookNames = _productService.GetBookNamesForAutoCompleteSearch(term).ToList();
            return Ok(bookNames);
        }

        #endregion
    }
}
