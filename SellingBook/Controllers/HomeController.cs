using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SellingBook.Models;
using Microsoft.Extensions.Logging;
using SellingBook.Repositories;
using SellingBook.Services;

namespace SellingBook.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IProductRepository _productRepository;
    private readonly IUserService _userService;

    public HomeController(ILogger<HomeController> logger, IProductRepository productRepository, IUserService userService)
    {
        _logger = logger;
        _productRepository = productRepository;
        _userService = userService;
    }

    public async Task<IActionResult> Index()
    {
        var products = await _productRepository.GetAllProductsAsync();
        ViewBag.CartQuantity = _userService.GetCartItemsCount();
        return View(products);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
    
}
