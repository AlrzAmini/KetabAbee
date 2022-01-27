using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KetabAbee.Application.Interfaces.Order;
using KetabAbee.Domain.Interfaces;

namespace KetabAbee.Application.Services.Order
{
    public class OrderService : IOrderService
    {
        #region constructor

        private readonly IOrderRepository _orderRepository;

        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        #endregion
    }
}
