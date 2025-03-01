using Microsoft.AspNetCore.Mvc;
using SellingBook.Models;
using SellingBook.Repositories;

namespace SellingBook.Controllers
{
    public class CartController : Controller
    {
        private ILogger<CartController> _logger;
        private readonly ICartRepository _userService;
        public CartController(ICartRepository userService, ILogger<CartController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [HttpPost]
        public IActionResult AddCartItem([FromBody] CartItem cartItem)
        {
            _logger.LogInformation(cartItem.UserId.ToString());
            _userService.AddCartItem(cartItem);
            return Ok(new
            {
                cartQuantity = _userService.GetCartItemsCountBasedOnRealTotal()
            });
        }
    }
}
