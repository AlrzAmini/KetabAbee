using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KetabAbee.Application.DTOs.Admin.Order;
using KetabAbee.Application.DTOs.Admin.Products.Book;
using KetabAbee.Application.DTOs.Paging;
using KetabAbee.Application.Interfaces.Order;
using KetabAbee.Application.Interfaces.Product;
using KetabAbee.Application.Interfaces.Wallet;
using KetabAbee.Domain.Interfaces;
using KetabAbee.Domain.Models.Order;
using KetabAbee.Domain.Models.Wallet;

namespace KetabAbee.Application.Services.Order
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IProductService _productService;
        private readonly IWalletService _walletService;

        public OrderService(IOrderRepository orderRepository, IProductService productService, IWalletService walletService)
        {
            _orderRepository = orderRepository;
            _productService = productService;
            _walletService = walletService;
        }

        public int AddOrder(int userId, int productId)
        {
            var order = _orderRepository.GetOrders().FirstOrDefault(o => o.UserId == userId && !o.IsFinally);
            var product = _productService.GetBookById(productId);
            var productInventory = product.Inventory;
            // check inventory
            if (productInventory != null && productInventory != 0)
            {
                // have not open order
                if (order == null)
                {
                    order = new Domain.Models.Order.Order
                    {
                        UserId = userId,
                        IsFinally = false,
                        CreateDate = DateTime.Now,
                        OrderSum = product.Price,
                        OrderDetails = new List<OrderDetail>
                        {
                            new()
                            {
                                ProductId = productId,
                                Price = product.Price,
                                Count = 1
                            }
                        }
                    };
                    _orderRepository.AddOrder(order);
                    _orderRepository.SaveChanges();
                }
                else //have open order
                {
                    var detail = _orderRepository.GetOrdersDetails()
                        .FirstOrDefault(d => d.OrderId == order.OrderId && d.ProductId == productId);
                    
                    if (detail != null)
                    {
                        detail.Count += 1;
                        //Check Inventory is enough for shopping
                        //if (detail.Count <= productInventory)
                        //{
                        //    UpdateDetail(detail);
                        //    product.Inventory -= 1;
                        //    _productService.UpdateBook(product);
                        //}
                        //else
                        //{
                        //    return -1;
                        //}
                        UpdateDetail(detail);
                        _orderRepository.SaveChanges();

                    }
                    else
                    {
                        detail = new OrderDetail
                        {
                            OrderId = order.OrderId,
                            Count = 1,
                            ProductId = productId,
                            Price = product.Price
                        };
                        _orderRepository.AddDetail(detail);
                    }

                    _orderRepository.UpdatePriceOrder(order.OrderId);
                }
            }
            else
            {
                return -1;
            }
            return order.OrderId;
        }

        public Domain.Models.Order.Order GetOrderForShowToUser(int userId, int orderId)
        {
            return _orderRepository.GetOrderForShowInUserPanel(userId, orderId);
        }

        public OrdersForShowInAdminViewModel GetPayedOrdersForAdmin(OrdersForShowInAdminViewModel ordersViewModel)
        {
            var result = _orderRepository.GetPayedOrdersForAdmin().AsQueryable();

            // Filter By Is Completed
            if (ordersViewModel.IsCompleted)
            {
                result = result.Where(r => !r.SendingProcessIsCompleted);
            }

            // paging
            var pager = Pager.Build(ordersViewModel.PageNum, result.Count(), ordersViewModel.Take, ordersViewModel.PageCountAfterAndBefor);
            var orders = result.Paging(pager).ToList();

            return ordersViewModel.SetPaging(pager).SetOrders(orders);
        }

        public IEnumerable<Domain.Models.Order.Order> GetUserFinalOrders(int userId)
        {
            return _orderRepository.GetUserFinalOrders(userId);
        }

        public IEnumerable<Domain.Models.Order.Order> GetUserOrders(int userId)
        {
            return _orderRepository.GetUserOrders(userId);
        }

        public bool PayByOrderId(int userId, int orderId)
        {
            var order = _orderRepository.GetOrdersWithIncludes()
                .FirstOrDefault(o => o.UserId == userId && o.OrderId == orderId);

            if (order == null || order.IsFinally)
            {
                return false;
            }

            if (_walletService.BalanceUserWallet(userId) < order.OrderSum) return false;

            order.IsFinally = true;
            _walletService.AddWallet(new Domain.Models.Wallet.Wallet
            {
                UserId = userId,
                Amount = order.OrderSum,
                Behalf = $"تصویه فاکتور شماره {orderId}",
                IsPay = true,
                CreateDate = DateTime.Now,
                WalletType = WalletType.Withdraw,
            });
            UpdateOrder(order);

            foreach (var detail in order.OrderDetails)
            {
               var book = _productService.GetBookById(detail.ProductId);
               _productService.DecreaseInventory(new ChangeInventoryViewModel
               {
                   BookId = book.BookId,
                   BookName = book.Name,
                   DecNumber = detail.Count
               });
                _productService.UpdateBook(book);
            }

            
            _orderRepository.AddUserBooks(order.OrderDetails);

            return true;

        }

        public bool RemoveItemOfOrderDetail(int userId, int orderId, int detailId)
        {
            var order = _orderRepository.GetOrdersForValidationInRemoveMethod().SingleOrDefault(o =>
                !o.IsFinally && o.OrderId == orderId && o.UserId == userId);

            if (order == null)
            {
                return false;
            }

            if (order.OrderDetails.All(d => d.DetailId != detailId))
            {
                return false;
            }

            var detail = _orderRepository.GetDetailById(detailId);
            return _orderRepository.RemoveItemOfOrderDetail(detail);
        }

        public void UpdateDetail(OrderDetail detail)
        {
            _orderRepository.UpdateDetail(detail);
        }

        public string UpdateDetailCount(int userId, int orderId, int detailId, int newCount)
        {
            try
            {
                var detail = _orderRepository.GetDetailsWithIncludes()
                    .SingleOrDefault(d => !d.Order.IsFinally && d.OrderId == orderId && d.DetailId == detailId);
                if (detail == null)
                {
                    return "Null";
                }

                if (newCount > detail.Product.Inventory)
                {
                    return "OutOfRange";
                }
                detail.Count = newCount;
                UpdateDetail(detail);
                _orderRepository.UpdatePriceOrder(orderId);
                return "Success";
            }
            catch
            {
                return "Exception";
            }
        }

        public bool UpdateOrder(Domain.Models.Order.Order order)
        {
            try
            {
                _orderRepository.UpdateOrder(order);
                _orderRepository.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
