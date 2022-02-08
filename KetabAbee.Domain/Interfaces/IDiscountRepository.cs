using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KetabAbee.Domain.Models.Products;

namespace KetabAbee.Domain.Interfaces
{
    public interface IDiscountRepository
    {
        IEnumerable<ProductDiscount> GetDiscounts();

        bool AddDiscount(ProductDiscount discount);

        ProductDiscount GetDiscountById(int discountId);

        bool UpdateDiscount(ProductDiscount discount);

        bool RemoveDiscount(ProductDiscount discount);
    }
}
