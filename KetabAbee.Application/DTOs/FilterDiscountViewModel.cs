using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KetabAbee.Application.DTOs.Paging;
using KetabAbee.Domain.Models.Products;

namespace KetabAbee.Application.DTOs
{
    public class FilterDiscountViewModel : BasePaging
    {
        #region props

        public int? ProductId { get; set; }

        public List<ProductDiscount> ProductDiscounts { get; set; }

        #endregion

        #region methods

        public FilterDiscountViewModel SetDiscounts(List<ProductDiscount> discounts)
        {
            ProductDiscounts = discounts;
            return this;
        }

        public FilterDiscountViewModel SetPaging(BasePaging paging)
        {
            PageNum = paging.PageNum;
            TotalEntitiesCount = paging.TotalEntitiesCount;
            StartPage = paging.StartPage;
            EndPage = paging.EndPage;
            PageCountAfterAndBefor = paging.PageCountAfterAndBefor;
            Take = paging.Take;
            Skip = paging.Skip;
            TotalPages = paging.TotalPages;

            return this;
        }

        #endregion
    }
}
