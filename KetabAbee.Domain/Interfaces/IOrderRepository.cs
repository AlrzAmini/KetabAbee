using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using KetabAbee.Domain.Models.Order;
using KetabAbee.Domain.Models.Products;

namespace KetabAbee.Domain.Interfaces
{
    public interface IOrderRepository
    {
        #region order

        IEnumerable<Order> GetOrders();

        IEnumerable<OrderDetail> GetOrdersDetails();

        int AddOrder(Order order);

        void UpdateDetail(OrderDetail detail);

        void UpdateOrder(Order order);

        int AddDetail(OrderDetail detail);

        void SaveChanges();

        Order GetOrderById(int orderId);

        Order GetOrderByIdForUpdatePrice(int orderId);

        void UpdatePriceOrder(int orderId);

        void UpdateBook(Book book);

        Order GetOrderForShowInUserPanel(int userId, int orderId);

        IEnumerable<Order> GetUserOrders(int userId);

        bool RemoveItemOfOrderDetail(OrderDetail detail);

        IEnumerable<Order> GetOrdersForValidationInRemoveMethod();

        OrderDetail GetDetailById(int detailId);

        IEnumerable<OrderDetail> GetDetailsWithIncludes();

        IEnumerable<Order> GetOrdersWithIncludes();

        IEnumerable<Order> GetUserFinalOrders(int userId);

        IEnumerable<Order> GetPayedOrdersForAdmin();

        #endregion

        void AddUserBooks(ICollection<OrderDetail> orderDetails);
    }
}
