using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KetabAbee.Application.DTOs;
using KetabAbee.Application.DTOs.Discount;
using KetabAbee.Domain.Models.Products;

namespace KetabAbee.Application.Interfaces.Product.Discount
{
    public interface IDiscountService
    {
        FilterDiscountViewModel FilterDiscount(FilterDiscountViewModel filter);

        CreateDiscountResult Create(CreateDiscountViewModel discount);

        ProductDiscount GetDiscountById(int discountId);

        bool UpdateDiscount(int discountId);

        bool RemoveDiscount(int discountId);
    }
}
