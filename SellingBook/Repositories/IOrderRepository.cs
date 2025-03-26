using SellingBook.Models;
using SellingBook.Models.BasicModels;
using System;

namespace SellingBook.Repositories
{
    public interface IOrderRepository
    {
        // Phương thức lưu bình luận và trả về đối tượng ProductComment
        ProductComment SaveComment(string orderId, string comment, int productId, string userId);
        int GetOrdersCountBasedOnIds();
        void AddOrder(Order order);
        void DeleteOrder(Order order);
        IEnumerable<Order> GetOrders();
    }
}
