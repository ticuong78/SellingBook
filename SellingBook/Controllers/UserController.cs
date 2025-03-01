using SellingBook.Services;
using Microsoft.AspNetCore.Mvc;
using SellingBook.Models;

namespace SellingBook.Controllers
{
    public class UserController : Controller
    {
        private ILogger<UserController> _logger;
        private readonly IUserService _userService;
        public UserController(IUserService userService, ILogger<UserController> logger)
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
                cartQuantity = _userService.GetCartItemsCount()
            });
        }
    }
}
