using SellingBook.Models;
using SellingBook.Models.BasicModels;
using SellingBook.Models.Identity;
using SellingBook.Services.User;
using System.Security.Claims;

namespace SellingBook.Repositories
{
    public class EFCartRepository : ICartRepository
    {
        private readonly ILogger<EFCartRepository> _logger;
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IUserService _userService;
        public EFCartRepository(
            ApplicationDbContext applicationDbContext, 
            ILogger<EFCartRepository> logger,
            IUserService userService
        )
        {
            _userService = userService;
            _applicationDbContext = applicationDbContext;
            _logger = logger;
        }

        public void AddCartItem(CartItem cartItem)
        {
            ApplicationUser user = _applicationDbContext.Users.FirstOrDefault(u => u.Email == _userService.GetCurrentUserEmail());
            Product product = _applicationDbContext.Products.FirstOrDefault(product => product.ProductId == cartItem.ProductId);
            CartItem existingCartItem = _applicationDbContext.CartItems.FirstOrDefault(item => item.ProductId == cartItem.ProductId);

            if (existingCartItem != null)
            {
                existingCartItem.CartItemQuantity += cartItem.CartItemQuantity;
                existingCartItem.CartItemPrice += cartItem.CartItemPrice;
                existingCartItem.User = user;
                existingCartItem.UserId = user.Id;
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
                cartItem.UserId = user.Id;
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
            return _applicationDbContext.CartItems.Where(cartItem => cartItem.User.Email == _userService.GetCurrentUserEmail());
        }

        public int GetCartItemsCountBasedOnIds()
        {
            return _applicationDbContext.CartItems.Where(cartItem => cartItem.User.Email == _userService.GetCurrentUserEmail()).Count();
        }

        public int GetCartItemsCountBasedOnRealTotal()
        {
            return _applicationDbContext.CartItems.Where(cartItem => cartItem.User.Email == _userService.GetCurrentUserEmail()).Sum(cart => cart.CartItemQuantity);
        }
    }
}
