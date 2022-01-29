using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KetabAbee.Data.Context;
using KetabAbee.Domain.Interfaces;
using KetabAbee.Domain.Models.Order;
using KetabAbee.Domain.Models.Products;
using Microsoft.EntityFrameworkCore;

namespace KetabAbee.Data.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly KetabAbeeDBContext _context;

        public OrderRepository(KetabAbeeDBContext context)
        {
            _context = context;
        }

        public void UpdateOrder(Order order)
        {
            _context.Orders.Update(order);
        }

        public int AddDetail(OrderDetail detail)
        {
            _context.OrderDetails.Add(detail);
            _context.SaveChanges();
            return detail.DetailId;
        }

        public int AddOrder(Order order)
        {
            _context.Orders.Add(order);
            _context.SaveChanges();
            return order.OrderId;
        }

        public Order GetOrderById(int orderId)
        {
            return _context.Orders.Find(orderId);
        }

        public Order GetOrderByIdForUpdatePrice(int orderId)
        {
            return _context.Orders
                .Include(o=>o.OrderDetails.Where(d=>d.OrderId == orderId))
                .SingleOrDefault(o=>o.OrderId == orderId);
        }

        public IEnumerable<Order> GetOrders()
        {
            return _context.Orders;
        }

        public IEnumerable<OrderDetail> GetOrdersDetails()
        {
            return _context.OrderDetails;
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public void UpdateDetail(OrderDetail detail)
        {
            _context.OrderDetails.Update(detail);
            _context.SaveChanges();
        }

        public void UpdatePriceOrder(int orderId)
        {
            var order = GetOrderByIdForUpdatePrice(orderId);
            order.OrderSum = order.OrderDetails.Sum(detail => detail.Price * detail.Count);
            _context.Orders.Update(order);
            _context.SaveChanges();
        }

        public void UpdateBook(Book book)
        {
            _context.Books.Update(book);
            _context.SaveChanges();
        }
    }
}
