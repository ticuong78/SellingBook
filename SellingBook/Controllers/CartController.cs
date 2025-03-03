using Microsoft.AspNetCore.Mvc;
using SellingBook.Models;
using SellingBook.Repositories;

namespace SellingBook.Controllers
{
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

        [HttpGet]
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
            _logger.LogInformation(cartItem.UserId.ToString());
            _cartRepository.AddCartItem(cartItem);
            return Ok(new
            {
                cartQuantity = _cartRepository.GetCartItemsCountBasedOnRealTotal()
            });
        }
    }
}
