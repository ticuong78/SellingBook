using SellingBook.Models;
using SellingBook.Models.BasicModels;
using SellingBook.Repositories;
using System.Collections.Generic;

namespace SellingBook.Services.OrderSe
{
    public class OrderService
    {
        private readonly IOrderRepository _orderRepository;

        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        // Thêm đơn hàng mới
        public void CreateOrder(Order order)
        {
            _orderRepository.AddOrder(order);
        }

        // Xóa đơn hàng theo đối tượng
        public void RemoveOrder(Order order)
        {
            _orderRepository.DeleteOrder(order);
        }

        // Lấy danh sách tất cả đơn hàng
        public IEnumerable<Order> GetAllOrders()
        {
            return _orderRepository.GetOrders();
        }

        // Đếm số lượng đơn hàng trong hệ thống
        public int GetOrderCount()
        {
            return _orderRepository.GetOrdersCountBasedOnIds();
        }

        // Lưu bình luận vào đơn hàng
        public ProductComment AddCommentToOrder(string orderId, string comment, int productId, string userId)
        {
            return _orderRepository.SaveComment(orderId, comment, productId, userId);
        }
    }
}
