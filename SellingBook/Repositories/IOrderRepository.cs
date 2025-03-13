using SellingBook.Models.BasicModels;

namespace SellingBook.Repositories
{
    public interface IOrderRepository
    {
        int GetOrdersCountBasedOnIds();
        void AddOrder(Order order);
        void DeleteOrder(Order order);
        IEnumerable<Order> GetOrders();
    }
}
