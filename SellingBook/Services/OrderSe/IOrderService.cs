using SellingBook.Models;
using SellingBook.Models.BasicModels;

namespace SellingBook.Services
{
    public interface IOrderService
    {
        public void AddOrder(string orderId, string sessionKey, ISession? session = null);
        IEnumerable<Order> GetAllOrders();
        ProductComment SaveComment(string orderId, string comment, int productId, string userId);
    }
}
