using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Host;
using SellingBook.Models.Error;
using SellingBook.Repositories;
using SellingBook.Services.ChangeLanguage;
using System.Diagnostics;
namespace SellingBook.Controllers;

[AllowAnonymous]
public class HomeController : Controller
{
    private readonly ICartRepository _cartRepository;
    private readonly IChangeLanguageService _languageService;

    public HomeController(ICartRepository cartRepository, IChangeLanguageService languageService)
    {
        _cartRepository = cartRepository;
        _languageService = languageService;
    }

    public async Task<IActionResult> Index()
    {
        ViewData["CartQuantity"] = _cartRepository.GetCartItemsCountBasedOnRealTotal();

        return View();
    }

    public IActionResult ChangeLanguage(string culture)
    {
        _languageService.SetLanguage(HttpContext, culture);
        return Redirect(Request.Headers["Referer"].ToString()); // Redirect to the last visited page
    }

    public IActionResult Error()
    {
        var errorViewModel = new ErrorViewModel
        {
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
        };

        return View(errorViewModel);
    }
}

