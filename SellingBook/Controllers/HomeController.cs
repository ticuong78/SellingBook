using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SellingBook.Repositories;
namespace SellingBook.Controllers;

[AllowAnonymous]
public class HomeController : Controller
{
    private readonly ICartRepository _cartRepository;
    public HomeController(ICartRepository cartRepository)
    {
        _cartRepository = cartRepository;
    }

    public async Task<IActionResult> Index()
    {
        ViewData["CartQuantity"] = _cartRepository.GetCartItemsCountBasedOnRealTotal();

        return View();
    }
}
