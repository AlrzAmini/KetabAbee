using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KetabAbee.Application.Convertors;
using KetabAbee.Application.DTOs;
using KetabAbee.Application.DTOs.Admin.Discount;
using KetabAbee.Application.DTOs.Discount;
using KetabAbee.Application.DTOs.Paging;
using KetabAbee.Application.Interfaces.Product;
using KetabAbee.Application.Interfaces.Product.Discount;
using KetabAbee.Domain.Interfaces;
using KetabAbee.Domain.Models.Products;

namespace KetabAbee.Application.Services.Product.Discount
{
    public class DiscountService : IDiscountService
    {
        private readonly IDiscountRepository _discountRepository;
        private readonly IProductService _productService;

        public DiscountService(IDiscountRepository discountRepository, IProductService productService)
        {
            _discountRepository = discountRepository;
            _productService = productService;
        }

        public CreateDiscountResult Create(CreateDiscountViewModel discount)
        {
            try
            {
                var product = _productService.GetBookById(discount.productId);
                if (product == null)
                    return CreateDiscountResult.NotFound;

                var newDiscount = new ProductDiscount
                {
                    ProductId = discount.productId,
                    DiscountNumber = discount.DiscountNumber,
                    Percent = discount.Percent,
                    ExpiredDate = discount.ExpiredDate.ToMiladiDateTime(),
                    StartDate = DateTime.Now
                };

                return _discountRepository.AddDiscount(newDiscount) ? CreateDiscountResult.Success : CreateDiscountResult.Error;
            }
            catch
            {
                return CreateDiscountResult.Error;
            }
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

        public ProductDiscount GetDiscountById(int discountId)
        {
            return _discountRepository.GetDiscountById(discountId);
        }

        public bool RemoveDiscount(int discountId)
        {
            return _discountRepository.RemoveDiscount(GetDiscountById(discountId));
        }

        public EditDiscountViewModel GetInfoForEditDiscount(int discountId)
        {
            return _discountRepository.GetDiscounts()
                  .Where(d => d.DiscountId == discountId)
                  .Select(d => new EditDiscountViewModel()
                  {
                      ProductId = d.ProductId,
                      DiscountId = d.DiscountId,
                      DiscountNumber = d.DiscountNumber,
                      Percent = d.Percent,
                      ExpiredDate = d.ExpiredDate.ToShamsi(),
                      ProductName = d.Product.Name
                  }).Single();
        }

        public bool UpdateDiscount(int discountId)
        {
            return _discountRepository.UpdateDiscount(GetDiscountById(discountId));
        }

        public EditDiscountResult EditDiscount(EditDiscountViewModel discount)
        {
            try
            {
                var newDiscount = GetDiscountById(discount.DiscountId);

                if (newDiscount == null)
                {
                    return EditDiscountResult.NotFound;
                }

                newDiscount.ProductId = discount.ProductId;
                newDiscount.DiscountNumber = discount.DiscountNumber;
                newDiscount.ExpiredDate = discount.ExpiredDate.ToMiladiDateTime();
                newDiscount.Percent = discount.Percent;

                return _discountRepository.UpdateDiscount(newDiscount) ? EditDiscountResult.Success : EditDiscountResult.Error;
            }
            catch 
            {
                return EditDiscountResult.UnHandledException;
            }

        }
    }
}
