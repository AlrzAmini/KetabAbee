using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KetabAbee.Application.DTOs.Admin.Home;
using KetabAbee.Application.DTOs.Admin.Order;
using KetabAbee.Domain.Models.Order;

namespace KetabAbee.Application.Interfaces.Order
{
    public interface IOrderService
    {
        int AddOrder(int userId, int productId);

        void UpdateDetail(OrderDetail detail);

        Domain.Models.Order.Order GetOrderForShowToUser(int userId, int orderId);

        IEnumerable<Domain.Models.Order.Order> GetUserOrders(int userId);

        bool RemoveItemOfOrderDetail(int userId, int orderId, int detailId);

        ChangeCountResult UpdateDetailCount(int userId, int orderId, int detailId, int newCount);

        Task<bool> PayByOrderId(int userId, int orderId);

        bool UpdateOrder(Domain.Models.Order.Order order);

        IEnumerable<Domain.Models.Order.Order> GetUserFinalOrders(int userId);

        OrdersForShowInAdminViewModel GetPayedOrdersForAdmin(OrdersForShowInAdminViewModel ordersViewModel);

        bool ChangeIsCompleted(int orderId);

        Domain.Models.Order.Order GetOrderByIdForShowInfo(int orderId);

        bool AddOrderAddress(int orderId, int userId, string address);

        Domain.Models.Order.Order GetOrderById(int orderId);

        Task<Domain.Models.Order.Order> GetUserUnFinalOrder(int userId);

        #region Admin

        Task<SellStaticsViewModel> GetSellInfo();

        Task<int> GetIsSendOrdersPercent();

        Task<int> GetIsNotSendOrdersPercent();

        #endregion
    }
}
