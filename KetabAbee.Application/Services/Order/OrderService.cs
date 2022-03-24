using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KetabAbee.Application.DTOs.Admin.Home;
using KetabAbee.Application.DTOs.Admin.Order;
using KetabAbee.Application.DTOs.Admin.Products.Book;
using KetabAbee.Application.DTOs.Paging;
using KetabAbee.Application.Interfaces.Order;
using KetabAbee.Application.Interfaces.Product;
using KetabAbee.Application.Interfaces.Wallet;
using KetabAbee.Application.Senders;
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
            // include discount toye product va gereftanesh toye sabt detail
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

        public bool AddOrderAddress(int orderId, int userId, string address)
        {
            var order = GetOrderById(orderId);

            if (order.UserId != userId) return false;
            if (string.IsNullOrEmpty(address)) return false;

            order.Address = address;
            UpdateOrder(order);
            return true;

        }

        public bool ChangeIsCompleted(int orderId)
        {
            try
            {
                var order = _orderRepository.GetOrderById(orderId);

                if (order == null) return false;

                if (order.SendingProcessIsCompleted)
                {
                    order.SendingProcessIsCompleted = false;
                    return UpdateOrder(order);
                }
                order.SendingProcessIsCompleted = true;
                return UpdateOrder(order);
            }
            catch
            {
                return false;
            }

        }

        public async Task<int> GetIsNotSendOrdersPercent()
        {
            var allOrdersCount = await _orderRepository.AllOrdersCount();
            var isNotSendOrdersCount = await _orderRepository.IsNotSendOrderPercentCount();
            if (isNotSendOrdersCount == 0)
            {
                return 0;
            }

            return (isNotSendOrdersCount * 100) / allOrdersCount;
        }

        public async Task<int> GetIsSendOrdersPercent()
        {
            var allOrdersCount = await _orderRepository.AllOrdersCount();
            var isSendOrdersCount = await _orderRepository.IsSendOrdersCount();
            if (isSendOrdersCount == 0)
            {
                return 0;
            }

            return (isSendOrdersCount * 100) / allOrdersCount;
        }

        public Domain.Models.Order.Order GetOrderById(int orderId)
        {
            return _orderRepository.GetOrderById(orderId);
        }

        public Domain.Models.Order.Order GetOrderByIdForShowInfo(int orderId)
        {
            return _orderRepository.GetOrderByIdForShowInfo(orderId);
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

        public async Task<SellStaticsViewModel> GetSellInfo()
        {
            return new SellStaticsViewModel
            {
                LastDayOrdersCount = await _orderRepository.GetLastNDaysOrdersCount(1),
                LastWeekOrdersCount = await _orderRepository.GetLastNDaysOrdersCount(7),
                LastMonthOrdersCount = await _orderRepository.GetLastNDaysOrdersCount(30),
                LastYearOrdersCount = await _orderRepository.GetLastNDaysOrdersCount(365),
                LastDayOrdersIncome = await _orderRepository.GetLastNDaysOrdersIncome(1),
                LastWeekOrdersIncome = await _orderRepository.GetLastNDaysOrdersIncome(7),
                LastMonthOrdersIncome = await _orderRepository.GetLastNDaysOrdersIncome(30),
                LastYearOrdersIncome = await _orderRepository.GetLastNDaysOrdersIncome(365),
                MostSellingBook = await _orderRepository.GetMostSellingBooks(),
                MostSellingBookCategories = await _orderRepository.GetMostSellingBookCategories(),
                IsSendPercent = await GetIsSendOrdersPercent(),
                IsNotSendPercent = await GetIsNotSendOrdersPercent()
            };
        }

        public IEnumerable<Domain.Models.Order.Order> GetUserFinalOrders(int userId)
        {
            return _orderRepository.GetUserFinalOrders(userId);
        }

        public IEnumerable<Domain.Models.Order.Order> GetUserOrders(int userId)
        {
            return _orderRepository.GetUserOrders(userId);
        }

        public async Task<Domain.Models.Order.Order> GetUserUnFinalOrder(int userId)
        {
            return await _orderRepository.GetUserUnFinalOrder(userId);
        }

        public async Task<bool> PayByOrderId(int userId, int orderId)
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
            await SendEmail.Send(order.User.Email, "خرید موفق از کتاب آبی", "سفارش شما در کتاب آبی با موفقیت ثبت گردید");
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

        public ChangeCountResult UpdateDetailCount(int userId, int orderId, int detailId, int newCount)
        {
            try
            {
                var detail = _orderRepository.GetDetailsWithIncludes()
                    .SingleOrDefault(d => !d.Order.IsFinally && d.OrderId == orderId && d.DetailId == detailId);
                if (detail == null)
                {
                    return ChangeCountResult.NotFound;
                }

                if (newCount > detail.Product.Inventory)
                {
                    return ChangeCountResult.OutOfRange;
                }
                detail.Count = newCount;
                UpdateDetail(detail);
                _orderRepository.UpdatePriceOrder(orderId);
                return ChangeCountResult.Success;
            }
            catch
            {
                return ChangeCountResult.UnHandledException;
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
