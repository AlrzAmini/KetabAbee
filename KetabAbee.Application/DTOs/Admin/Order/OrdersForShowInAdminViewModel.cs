using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KetabAbee.Application.DTOs.Paging;

namespace KetabAbee.Application.DTOs.Admin.Order
{
    public class OrdersForShowInAdminViewModel : BasePaging
    {
        public bool IsCompleted { get; set; }

        public List<Domain.Models.Order.Order> Orders { get; set; }

        #region Methods

        public OrdersForShowInAdminViewModel SetOrders(List<Domain.Models.Order.Order> orders)
        {
            Orders = orders;
            return this;
        }

        public OrdersForShowInAdminViewModel SetPaging(BasePaging paging)
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
