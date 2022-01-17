using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KetabAbee.Domain.Models.Products;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace KetabAbee.Application.Interfaces.Product
{
    public interface IProductService
    {
        #region Groups

        IEnumerable<ProductGroup> GetGroups();

        IEnumerable<ProductGroup> GetGroupsForAdmin();

        bool AddGroup(ProductGroup group);

        bool UpdateGroup(ProductGroup group);

        ProductGroup GetGroupById(int groupId);

        bool DeleteGroupById(int groupId);

        #endregion

        #region Book

        IEnumerable<Publisher> GetPublishers();

        List<SelectListItem> GetGroupsForAddBook();

        List<SelectListItem> GetSubGroupsForAddBook(int groupId);

        bool AddBook(Book book,IFormFile imgFile);

        IEnumerable<Book> GetBooksForAdmin();

        #endregion
    }
}
