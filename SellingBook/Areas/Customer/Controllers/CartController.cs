using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SellingBook.Models.BasicModels;
using SellingBook.Models.Checkout;
using SellingBook.Models.Roles;
using SellingBook.Repositories;

namespace SellingBook.Areas.Customer.Controllers
{
    [Area(SD.Role_Customer)]
    [Authorize]
    public class CartController : Controller
    {
        private ILogger<CartController> _logger;
        private readonly ICartRepository _cartRepository;
        private readonly IProductRepository _productRepository;

        public CartController(ICartRepository cartRepository, IProductRepository productRepository, ILogger<CartController> logger)
        {
            _cartRepository = cartRepository;
            _productRepository = productRepository;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            _logger.LogInformation("Cart Index");
            ProductCartItemViewModel productCartItemViewModel = new();

            productCartItemViewModel.CartItems = _cartRepository.GetCartItems();
            productCartItemViewModel.Products = await _productRepository.GetAllProductsAsync();

            return View(productCartItemViewModel);
        }

        [HttpPost]
        public IActionResult AddCartItem([FromBody] CartItem cartItem)
        {
            _cartRepository.AddCartItem(cartItem);
            return Ok(new
            {
                cartQuantity = _cartRepository.GetCartItemsCountBasedOnRealTotal()
            });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteCartItem(int cartItemId)
        {
            _logger.LogInformation(cartItemId.ToString());

            CartItem cartItem = _cartRepository.GetCartItems().FirstOrDefault(c => c.CartItemId == cartItemId);

            if (cartItem == null)
            {
                return NotFound();
            }

            _cartRepository.DeleteCartItem(cartItem);

            ProductCartItemViewModel productCartItemViewModel = new();

            productCartItemViewModel.CartItems = _cartRepository.GetCartItems();
            productCartItemViewModel.Products = await _productRepository.GetAllProductsAsync();

            return View("Index", productCartItemViewModel);
        }
    }
}
