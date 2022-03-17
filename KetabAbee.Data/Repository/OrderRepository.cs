using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KetabAbee.Data.Context;
using KetabAbee.Domain.Interfaces;
using KetabAbee.Domain.Models.Order;
using KetabAbee.Domain.Models.Products;
using KetabAbee.Domain.Models.User;
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
                .Include(o => o.OrderDetails.Where(d => d.OrderId == orderId))
                .SingleOrDefault(o => o.OrderId == orderId);
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

        public Order GetOrderForShowInUserPanel(int userId, int orderId)
        {
            return _context.Orders
                .Include(o => o.OrderDetails)
                .ThenInclude(d => d.Product)
                .ThenInclude(b => b.Publisher)
                .Include(o => o.OrderDetails)
                .ThenInclude(o => o.Product)
                .ThenInclude(b => b.ProductDiscounts)
                .Include(o => o.User)
                .FirstOrDefault(o => o.UserId == userId && o.OrderId == orderId);
        }

        public IEnumerable<Order> GetUserOrders(int userId)
        {
            return _context.Orders.Where(o => o.UserId == userId);
        }

        public bool RemoveItemOfOrderDetail(OrderDetail detail)
        {
            detail.Price = 0;
            detail.IsDelete = true;
            UpdateDetail(detail);
            UpdatePriceOrder(detail.OrderId);
            return true;
        }

        public IEnumerable<Order> GetOrdersForValidationInRemoveMethod()
        {
            return _context.Orders
                .Include(o => o.OrderDetails)
                .Include(o => o.User);
        }

        public OrderDetail GetDetailById(int detailId)
        {
            return _context.OrderDetails.Find(detailId);
        }

        public IEnumerable<OrderDetail> GetDetailsWithIncludes()
        {
            return _context.OrderDetails
                .Include(d => d.Order)
                .Include(d => d.Product);
        }

        public IEnumerable<Order> GetOrdersWithIncludes()
        {
            return _context.Orders
                .Include(o => o.User)
                .Include(o => o.OrderDetails).ThenInclude(o => o.Product);
        }

        public void AddUserBooks(ICollection<OrderDetail> orderDetails)
        {
            foreach (var detail in orderDetails)
            {
                _context.UserBooks.Add(new UserBook
                {
                    BookId = detail.ProductId,
                    UserId = detail.Order.UserId
                });
            }

            _context.SaveChanges();
        }

        public IEnumerable<Order> GetUserFinalOrders(int userId)
        {
            return _context.Orders.Where(o => o.UserId == userId && o.IsFinally)
                .OrderByDescending(o => o.CreateDate)
                .Take(10);
        }

        public IEnumerable<Order> GetPayedOrdersForAdmin()
        {
            return _context.Orders.Include(o => o.User)
                .Where(o => o.IsFinally)
                .OrderByDescending(o => o.CreateDate);
        }

        public Order GetOrderByIdForShowInfo(int orderId)
        {
            return _context.Orders
                .Include(o => o.OrderDetails)
                .ThenInclude(d => d.Product)
                .Include(o => o.User)
                .SingleOrDefault(o => o.OrderId == orderId);
        }

        public async Task<Order> GetUserUnFinalOrder(int userId)
        {
            return await _context.Orders
                .Include(o => o.OrderDetails)
                .ThenInclude(d => d.Product)
                .ThenInclude(p => p.Publisher)
                .FirstOrDefaultAsync(o => o.UserId == userId && !o.IsFinally);
        }

        public async Task<int> GetLastNDaysOrdersCount(int n)
        {
            return await _context.Orders.CountAsync(o => o.IsFinally && o.CreateDate >= DateTime.Now.AddDays(-n));
        }

        public async Task<float> GetLastNDaysOrdersIncome(int n)
        {
            return await _context.Orders
                .Where(o => o.IsFinally && o.CreateDate >= DateTime.Now.AddDays(-n))
                .SumAsync(o => o.OrderSum);
        }

        public async Task<List<string>> GetMostSellingBooks()
        {
            return await _context.Books
                .Include(b => b.OrderDetails)
                .ThenInclude(d => d.Order)
                .Where(b => b.OrderDetails.Any())
                .OrderByDescending(d => d.OrderDetails.Count)
                .Select(b => b.Name).Take(4).ToListAsync();
        }

        public async Task<List<string>> GetMostSellingBookCategories()
        {
            return await _context.Books
                .Include(b=>b.SubGroup)
                .Include(b => b.OrderDetails)
                .ThenInclude(d => d.Order)
                .Where(b => b.OrderDetails.Any())
                .OrderByDescending(d => d.OrderDetails.Count)
                .Select(b => b.SubGroup.GroupTitle).Distinct()
                .Take(4)
                .ToListAsync();
        }

        public async Task<int> IsSendOrdersCount()
        {
            return await _context.Orders.CountAsync(o => o.SendingProcessIsCompleted);
        }

        public async Task<int> IsNotSendOrderPercentCount()
        {
            return await _context.Orders.CountAsync(o => !o.SendingProcessIsCompleted);
        }

        public async Task<int> AllOrdersCount()
        {
            return await _context.Orders.CountAsync();
        }
    }
}
