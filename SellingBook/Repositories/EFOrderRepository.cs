using SellingBook.Models;
using SellingBook.Models.BasicModels;

namespace SellingBook.Repositories
{
    public class EFOrderRepository: IOrderRepository
    {
        private ApplicationDbContext _applicationDbContext;
        public EFOrderRepository(ApplicationDbContext applicationDbContext) { 
            _applicationDbContext = applicationDbContext;
        }

        public void AddOrder(Order order)
        {
            _applicationDbContext.Orders.Add(order);
        }

        public void DeleteOrder(Order order)
        {
            _applicationDbContext.Orders.Remove(order);
        }

        public IEnumerable<Order> GetOrders()
        {
            return _applicationDbContext.Orders;
        }

        public int GetOrdersCountBasedOnIds()
        {
            return 0;
        }
    }
}
