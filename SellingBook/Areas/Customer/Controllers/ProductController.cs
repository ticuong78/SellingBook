using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using SellingBook.Models.Error;
using SellingBook.Models.Roles;
using SellingBook.Repositories;
namespace SellingBook.Controllers;

[Area(SD.Role_Customer)]
[Authorize]
public class ProductController : Controller
{
    private readonly ILogger<ProductController> _logger;
    private readonly IProductRepository _productRepository;
    private readonly ICartRepository _userService;

    public ProductController(ILogger<ProductController> logger, IProductRepository productRepository, ICartRepository userService)
    {
        _logger = logger;
        _productRepository = productRepository;
        _userService = userService;
    }

    public async Task<IActionResult> Index()
    {
        var products = await _productRepository.GetAllProductsAsync();
        ViewBag.CartQuantity = _userService.GetCartItemsCountBasedOnRealTotal();
        return View(products);
    }

    public IActionResult Display(int id)
    {
        var product = _productRepository.GetProductByIdAsync(id).Result;
        return View(product);
    }
}
