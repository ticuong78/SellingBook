using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using SellingBook.Models.BasicModels;
using SellingBook.Models.Roles;
using SellingBook.Repositories;

namespace SellingBook.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = $"{SD.Role_Admin},{SD.Role_Employee}")]
    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;

        public ProductController(IProductRepository productRepository, ICategoryRepository categoryRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
        }

        public async Task<IActionResult> Index()
        {
            var products = await _productRepository.GetAllProductsAsync();
            return View(products);
        }

        public async Task<IActionResult> Display(int id)
        {
            var product = await _productRepository.GetProductByIdAsync(id);
            return View(product);
        }

        public async Task<IActionResult> Add()
        {
            var categories = await _categoryRepository.GetAllCategoriesAsync();
            ViewBag.Categories = new SelectList(categories, "CategoryId", "CategoryName");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddConfirmed(Product product, IFormFile imageUrl, List<IFormFile> images)
        {
            if (ModelState.IsValid)
            {
                if (imageUrl != null)
                    product.ImageUrl = await AddImage(imageUrl);

                if (images != null)
                {
                    product.Images = new List<string>();
                    foreach (var image in images)
                    {
                        product.Images.Add(await AddImage(image));
                    }
                }

                await _productRepository.AddProductAsync(product);
                return RedirectToAction("Index");
            }

            ViewBag.Categories = new SelectList(await _categoryRepository.GetAllCategoriesAsync(), "CategoryId", "CategoryName");
            return View("Add", product);
        }

        private async Task<string> AddImage(IFormFile image)
        {
            if (image != null)
            {
                var path = Path.Combine("wwwroot", "images", image.FileName);
                using var stream = new FileStream(path, FileMode.Create);
                await image.CopyToAsync(stream);
            }
            return "/images/" + image.FileName;
        }

        public async Task<IActionResult> Edit(int id)
        {
            var product = await _productRepository.GetProductByIdAsync(id);
            if (product == null)
                return NotFound();

            ViewBag.Categories = new SelectList(await _categoryRepository.GetAllCategoriesAsync(), "CategoryId", "CategoryName");
            return View(product);
        }
        
        [HttpPost]
        public async Task<IActionResult> EditConfirmed(Product product, IFormFile imageUrl)
        {
            ModelState.Remove("ImageURL");
            if (ModelState.IsValid)
            {
                if (imageUrl != null)
                {
                    product.ImageUrl = await AddImage(imageUrl);
                }
                else
                {
                    var oldProduct = await _productRepository.GetProductByIdAsync(product.ProductId);
                    product.ImageUrl = oldProduct.ImageUrl;
                }

                await _productRepository.UpdateProductAsync(product);
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Categories = new SelectList(await _categoryRepository.GetAllCategoriesAsync(), "CategoryId", "CategoryName");
            return View("Edit", product);
        }

        [Authorize(Roles = SD.Role_Admin)]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _productRepository.GetProductByIdAsync(id);
            return product == null ? NotFound() : View(product);
        }

        [HttpPost]
        [Authorize(Roles = SD.Role_Admin)]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _productRepository.DeleteProductByIdAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
