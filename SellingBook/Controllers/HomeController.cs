using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SellingBook.Models.Error;
using SellingBook.Repositories;
using System.Diagnostics;
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
    public IActionResult Error()
    {
        var errorViewModel = new ErrorViewModel
        {
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
        };

        return View(errorViewModel);  // Tr? v? View v?i ErrorViewModel
    }
}

