using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KetabAbee.Application.DTOs;
using KetabAbee.Application.DTOs.Paging;
using KetabAbee.Application.Interfaces.Product.Discount;
using KetabAbee.Domain.Interfaces;

namespace KetabAbee.Application.Services.Product.Discount
{
    public class DiscountService : IDiscountService
    {
        private readonly IDiscountRepository _discountRepository;

        public DiscountService(IDiscountRepository discountRepository)
        {
            _discountRepository = discountRepository;
        }

        public FilterDiscountViewModel FilterDiscount(FilterDiscountViewModel filter)
        {
            var result = _discountRepository.GetDiscounts().AsQueryable();

            if (filter.ProductId != null && filter.ProductId != 0)
            {
                result = result.Where(r => r.ProductId == filter.ProductId);
            }

            //paging
            var pager = Pager.Build(filter.PageNum, result.Count(), filter.Take, filter.PageCountAfterAndBefor);
            var discounts = result.Paging(pager).ToList();

            return filter.SetPaging(pager).SetDiscounts(discounts);
        }
    }
}
