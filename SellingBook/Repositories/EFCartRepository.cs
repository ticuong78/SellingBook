using SellingBook.Models;

namespace SellingBook.Repositories
{
    public class EFCartRepository : ICartRepository
    {
        private ILogger<EFCartRepository> _logger;
        private ApplicationDbContext _applicationDbContext;
        public EFCartRepository(ApplicationDbContext applicationDbContext, ILogger<EFCartRepository> logger)
        {
            _applicationDbContext = applicationDbContext;
            _logger = logger;
        }

        public void AddCartItem(CartItem cartItem)
        {
            _logger.LogInformation(cartItem.UserId.ToString());

            CartItem existingCartItem = _applicationDbContext.CartItems.FirstOrDefault(item => item.ProductId == cartItem.ProductId);

            if (existingCartItem != null)
            {
                existingCartItem.CartItemQuantity += cartItem.CartItemQuantity;
                existingCartItem.CartItemPrice += cartItem.CartItemPrice;
                _applicationDbContext.SaveChanges();
                return;
            }

            _applicationDbContext.CartItems.Add(cartItem);
            _applicationDbContext.SaveChanges();
        }

        public void DeleteCartItem(CartItem cartItem)
        {
            _applicationDbContext.CartItems.Remove(cartItem);
            _applicationDbContext.SaveChanges();
        }

        public int GetCartItemsCountBasedOnIds()
        {
            return _applicationDbContext.CartItems.Where(cartItem => cartItem.UserId == 1).Count();
        }

        public int GetCartItemsCountBasedOnRealTotal()
        {
            return _applicationDbContext.CartItems.Where(cartItem => cartItem.UserId == 1).Sum(cart => cart.CartItemQuantity);
        }
    }
}
