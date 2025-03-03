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

<<<<<<< HEAD
=======
            User user = _applicationDbContext.Users.FirstOrDefault(user => user.UserId == cartItem.UserId);
            Product product = _applicationDbContext.Products.FirstOrDefault(product => product.ProductId == cartItem.ProductId);
>>>>>>> feature/payment-methods-momo
            CartItem existingCartItem = _applicationDbContext.CartItems.FirstOrDefault(item => item.ProductId == cartItem.ProductId);

            if (existingCartItem != null)
            {
                existingCartItem.CartItemQuantity += cartItem.CartItemQuantity;
                existingCartItem.CartItemPrice += cartItem.CartItemPrice;
                _applicationDbContext.SaveChanges();
                return;
            }

<<<<<<< HEAD
=======
            if(cartItem.Product == null)
            {
                _logger.LogInformation("Product is null");
                cartItem.Product = product;
            }

            if (cartItem.User == null)
            {
                _logger.LogInformation("User is null");
                cartItem.User = user;
            }

>>>>>>> feature/payment-methods-momo
            _applicationDbContext.CartItems.Add(cartItem);
            _applicationDbContext.SaveChanges();
        }

        public void DeleteCartItem(CartItem cartItem)
        {
            _applicationDbContext.CartItems.Remove(cartItem);
            _applicationDbContext.SaveChanges();
        }

<<<<<<< HEAD
=======
        public IEnumerable<CartItem> GetCartItems()
        {
            return _applicationDbContext.CartItems.Where(cartItem => cartItem.UserId == 1);
        }

>>>>>>> feature/payment-methods-momo
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
