using Microsoft.AspNetCore.Mvc;
using SellingBook.Models;
using SellingBook.Repositories;

namespace SellingBook.Controllers
{
    public class CartController : Controller
    {
        private ILogger<CartController> _logger;
<<<<<<< HEAD
        private readonly ICartRepository _userService;
        public CartController(ICartRepository userService, ILogger<CartController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

=======
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

>>>>>>> feature/payment-methods-momo
        [HttpPost]
        public IActionResult AddCartItem([FromBody] CartItem cartItem)
        {
            _logger.LogInformation(cartItem.UserId.ToString());
<<<<<<< HEAD
            _userService.AddCartItem(cartItem);
            return Ok(new
            {
                cartQuantity = _userService.GetCartItemsCountBasedOnRealTotal()
=======
            _cartRepository.AddCartItem(cartItem);
            return Ok(new
            {
                cartQuantity = _cartRepository.GetCartItemsCountBasedOnRealTotal()
>>>>>>> feature/payment-methods-momo
            });
        }
    }
}
