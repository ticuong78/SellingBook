using SellingBook.Models;
using SellingBook.Models.BasicModels;

namespace SellingBook.Services
{
    public interface IOrderService
    {
        void CreateOrder(Order order);
        void RemoveOrder(Order order);
        IEnumerable<Order> GetAllOrders();
        int GetTotalOrders();
        ProductComment AddCommentToOrder(string orderId, string comment, int productId, string userId);
    }
}
