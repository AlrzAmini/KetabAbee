using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KetabAbee.Application.Interfaces.Order;
using KetabAbee.Application.Interfaces.Product;
using KetabAbee.Domain.Interfaces;
using KetabAbee.Domain.Models.Order;

namespace KetabAbee.Application.Services.Order
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IProductService _productService;

        public OrderService(IOrderRepository orderRepository, IProductService productService)
        {
            _orderRepository = orderRepository;
            _productService = productService;
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
                        product.Inventory -= 1;
                        _productService.UpdateBook(product);

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

        public void UpdateDetail(OrderDetail detail)
        {
            _orderRepository.UpdateDetail(detail);
        }
    }
}
