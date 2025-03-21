using SellingBook.Models;
using SellingBook.Models.BasicModels;
using SellingBook.Models.Identity;

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

            ApplicationUser user = _applicationDbContext.Users.FirstOrDefault(user => user.Id == cartItem.UserId);
            Product product = _applicationDbContext.Products.FirstOrDefault(product => product.ProductId == cartItem.ProductId);
            CartItem existingCartItem = _applicationDbContext.CartItems.FirstOrDefault(item => item.ProductId == cartItem.ProductId);

            if (existingCartItem != null)
            {
                existingCartItem.CartItemQuantity += cartItem.CartItemQuantity;
                existingCartItem.CartItemPrice += cartItem.CartItemPrice;
                _applicationDbContext.SaveChanges();
                return;
            }

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

            _applicationDbContext.CartItems.Add(cartItem);
            _applicationDbContext.SaveChanges();
        }

        public void DeleteCartItem(CartItem cartItem)
        {
            _applicationDbContext.CartItems.Remove(cartItem);
            _applicationDbContext.SaveChanges();
        }

        public IEnumerable<CartItem> GetCartItems()
        {
            return _applicationDbContext.CartItems.Where(cartItem => cartItem.UserId == "");
        }

        public int GetCartItemsCountBasedOnIds()
        {
            return _applicationDbContext.CartItems.Where(cartItem => cartItem.UserId == "").Count();
        }

        public int GetCartItemsCountBasedOnRealTotal()
        {
            return _applicationDbContext.CartItems.Where(cartItem => cartItem.UserId == "").Sum(cart => cart.CartItemQuantity);
        }
    }
}
