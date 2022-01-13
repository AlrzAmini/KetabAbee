using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KetabAbee.Domain.Models.Products;

namespace KetabAbee.Domain.Interfaces
{
   public interface IProductRepository
    {
        #region Product Group

        IEnumerable<ProductGroup> GetGroups();

        #endregion
    }
}
