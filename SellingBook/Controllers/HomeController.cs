using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Localization;
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

    public IActionResult ChangeLanguage(string culture)
    {
        Response.Cookies.Append(
            CookieRequestCultureProvider.DefaultCookieName,
            CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
            new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
        );
        return RedirectToAction("Index");
    }
}
