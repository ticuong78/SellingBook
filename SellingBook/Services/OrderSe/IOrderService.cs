using SellingBook.Models;
using SellingBook.Models.BasicModels;

namespace SellingBook.Services
{
    public interface IOrderService
    {
        void AddOrder(string orderId, string orderDescription, string cartSessionKey, ISession? session = null);
        void AddOrder(string orderId, string orderDescription, string couponSessionKey, string cartSessionKey, ISession? session = null);
        IEnumerable<Order> GetAllOrders();
        ProductComment SaveComment(string orderId, string comment, int productId, string userId);
    }
}
