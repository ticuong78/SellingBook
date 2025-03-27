using SellingBook.Extensions;
using SellingBook.Models;
using SellingBook.Models.BasicModels;
using SellingBook.Repositories;
using SellingBook.Services.User;

namespace SellingBook.Services.OrderSe
{
    public class OrderService: IOrderService
    {
        private readonly IUserService _userService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IOrderRepository _orderRepository;

        public OrderService(IOrderRepository orderRepository, IHttpContextAccessor httpContextAccessor, IUserService userService)
        {
            _orderRepository = orderRepository;
            _httpContextAccessor = httpContextAccessor;
            _userService = userService;
        }

        public void AddOrder(string orderId, string orderDescription, string cartSessionKey, ISession? session = null)
        {
            AddOrder(orderId, orderDescription, null, cartSessionKey, session);
        }

        public void AddOrder(string orderId, string orderDescription, string couponSessionKey, string cartSessionKey, ISession? session = null)
        {
            if(string.IsNullOrEmpty(orderId) || string.IsNullOrEmpty(cartSessionKey)) // Null or empty check
            {
                throw new InvalidOperationException("Invalid order id or session key.");
            }

            if(session == null) // Null check
            {
                session = _httpContextAccessor.HttpContext?.Session;
            }

            var cartItems = session?.GetObjectFromJson<List<CartItem>>(cartSessionKey);
            var couponId = session?.GetInt32(couponSessionKey);

            if (cartItems == null || !cartItems.Any()) // Null or empty check
            {
                throw new InvalidOperationException("Cart is empty or session expired.");
            }

            var order = new Order
            {
                OrderId = orderId,
                CouponId = couponId,
                OrderDescription = orderDescription == null ? "Không có mô tả" : orderDescription,
                UserId = _userService.GetCurrentUserId(),
                CreatedAt = DateTime.Now,
                TotalAmount = cartItems.Sum(x => x.CartItemPrice * x.CartItemQuantity),
                OrderItems = cartItems.Select(x => new OrderItem
                {
                    ProductId = x.ProductId,
                    OrderItemQuantity = x.CartItemQuantity,
                    OrderItemPrice = x.CartItemPrice,
                    OrderId = orderId,
                    OrderItemId = Guid.NewGuid().ToString(),
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
