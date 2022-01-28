using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KetabAbee.Domain.Models.Order;

namespace KetabAbee.Application.Interfaces.Order
{
    public interface IOrderService
    {
        int AddOrder(int userId, int productId);

        void UpdateDetail(OrderDetail detail);
    }
}
