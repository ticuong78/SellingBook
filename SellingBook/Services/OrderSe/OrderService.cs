using SellingBook.Extensions;
using SellingBook.Models;
using SellingBook.Models.BasicModels;
using SellingBook.Repositories;

namespace SellingBook.Services.OrderSe
{
    public class OrderService: IOrderService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IOrderRepository _orderRepository;

        public OrderService(IOrderRepository orderRepository, IHttpContextAccessor httpContextAccessor)
        {
            _orderRepository = orderRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public void AddOrder(string orderId, string sessionKey, ISession? session = null)
        {
            if(string.IsNullOrEmpty(orderId) || string.IsNullOrEmpty(sessionKey)) // Null or empty check
            {
                throw new InvalidOperationException("Invalid order id or session key.");
            }

            if(session == null) // Null check
            {
                session = _httpContextAccessor.HttpContext?.Session;
            }

            var cartItems = session?.GetObjectFromJson<List<CartItem>>(sessionKey);

            if (cartItems == null || !cartItems.Any()) // Null or empty check
            {
                throw new InvalidOperationException("Cart is empty or session expired.");
            }

            var order = new Order
            {
                OrderId = orderId,
                OrderItems = cartItems.Select(x => new OrderItem
                {
                    ProductId = x.ProductId,
                    OrderItemQuantity = x.CartItemQuantity,
                    OrderItemPrice = x.CartItemPrice,
                    OrderId = orderId,
                    OrderItemId = Guid.NewGuid().ToString()
                }).ToList()
            };

            _orderRepository.AddOrder(order);
        }

        public IEnumerable<Order> GetAllOrders()
        {
            return _orderRepository.GetOrders();
        }

        public ProductComment SaveComment(string orderId, string comment, int productId, string userId)
        {
            return _orderRepository.SaveComment(orderId, comment, productId, userId);
        }
    }
}
