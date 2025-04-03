using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Host;
using Microsoft.EntityFrameworkCore;
using SellingBook.Models.Error;
using SellingBook.Repositories;
using SellingBook.Services.ChangeLanguage;
using System.Diagnostics;
namespace SellingBook.Controllers;

[AllowAnonymous]
public class HomeController : Controller
{
    private readonly IProductRepository _productRepository;
    private readonly IChangeLanguageService _languageService;
    private readonly ICartRepository _cartRepository;
    private readonly ICategoryRepository _categoryRepository;

    public HomeController(
        IProductRepository productRepository, 
        IChangeLanguageService languageService, 
        ICartRepository cartRepository, 
        ICategoryRepository categoryRepository)
    {
        _productRepository = productRepository;
        _languageService = languageService;
        _cartRepository = cartRepository;
        _categoryRepository = categoryRepository;
    }

    public async Task<IActionResult> Index()
    {
        var products = await _productRepository.GetAllProductsAsync();
        var categories = await _categoryRepository.GetAllCategoriesAsync();
        ViewBag.CartQuantity = _cartRepository.GetCartItemsCountBasedOnRealTotal(); // Hiển thị tổng số lượng giỏ hàng
        return View(new
        {
            Products = products,
            Categories = categories
        });
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

