using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KetabAbee.Domain.Models.Products;

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

        #region Product

        

        #endregion
    }
}
