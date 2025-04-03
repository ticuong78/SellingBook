using SellingBook.Models.BasicModels;
using SellingBook.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using SellingBook.Models.Identity;

namespace SellingBook.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly ICartRepository _cartRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public ProductController(IProductRepository productRepository, ICartRepository cartRepository, UserManager<ApplicationUser> userManager)
        {
            _productRepository = productRepository;
            _cartRepository = cartRepository;
            _userManager = userManager;
        }

        // Phương thức hiển thị chi tiết sản phẩm
        [AllowAnonymous]
        public IActionResult Display(int id)
        {
            var product = _productRepository.GetProductByIdAsync(id).Result;
            return View(product);
        }

        // Phương thức thêm sản phẩm vào giỏ hàng
        [HttpPost("addtocart")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddToCart(int productId, int quantity = 1) // Default quantity is 1
        {
            var userId = _userManager.GetUserId(User);

            // Create CartItem with productId, quantity and price
            var cartItem = new CartItem
            {
                ProductId = productId,
                CartItemQuantity = quantity,
                CartItemPrice = (await _productRepository.GetProductByIdAsync(productId)).ProductPrice * quantity,
            };

            await _cartRepository.AddCartItem(cartItem); // Add the product to the cart
            return RedirectToAction("Index", "Home", new
            {
                area = ""
            }); // Redirect to Index after adding to cart
        }
    }
}
