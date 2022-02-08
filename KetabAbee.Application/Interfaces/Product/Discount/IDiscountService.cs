using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KetabAbee.Application.DTOs;

namespace KetabAbee.Application.Interfaces.Product.Discount
{
    public interface IDiscountService
    {
        FilterDiscountViewModel FilterDiscount(FilterDiscountViewModel filter);
    }
}
