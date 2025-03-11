using Microsoft.AspNetCore.Mvc;
using SellingBook.Repositories;
using Microsoft.AspNetCore.Mvc.Rendering;
using SellingBook.Models.BasicModels;
using SellingBook.Models;

namespace SellingBook.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
<<<<<<< HEAD
        private readonly ICartRepository _cartRepository;

        public ProductController(IProductRepository productRepository, ICategoryRepository categoryRepository, ICartRepository cartRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _cartRepository = cartRepository;
=======
        private readonly ApplicationDbContext _context;

        public ProductController(IProductRepository productRepository, ICategoryRepository categoryRepository, ApplicationDbContext context)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _context = context;
>>>>>>> f6604d8d2dad685e2ef187e4eb2dc4e6779aa3e9
        }

        public async Task<IActionResult> Index()
        {
            var products = await _productRepository.GetAllProductsAsync();
            return View(products);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.CategoryId = new SelectList(await _categoryRepository.GetAllCategoriesAsync(), "CategoryId", "CategoryName");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(Product product, IFormFile imageUrl)
        {
            if (ModelState.IsValid)
            {
                if (imageUrl != null)
                {
                    product.ImageUrl = await SaveImage(imageUrl);
                }

                await _productRepository.AddProductAsync(product);
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Categories = new SelectList(await _categoryRepository.GetAllCategoriesAsync(), "CategoryId", "CategoryName");
            return View(product);
        }

        private async Task<string> SaveImage(IFormFile image)
        {
            var savePath = Path.Combine("wwwroot/images", image.FileName);
            using (var fileStream = new FileStream(savePath, FileMode.Create))
            {
                await image.CopyToAsync(fileStream);
            }
            return "/images/" + image.FileName;
        }

        public async Task<IActionResult> Display(int productId)
        {
            var product = await _productRepository.GetProductByIdAsync(productId);
            if (product == null)
            {
                return NotFound();
            }

            ViewBag.CartQuantity = _cartRepository.GetCartItemsCountBasedOnRealTotal();
            return View(product);
        }

        public async Task<IActionResult> Update(int id)
        {
            var product = await _productRepository.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            ViewBag.Categories = new SelectList(await _categoryRepository.GetAllCategoriesAsync(), "CategoryId", "CategoryName", product.CategoryId);
            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, Product product, IFormFile imageUrl)
        {
            ModelState.Remove("ImageUrl");
            if (id != product.ProductId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var existingProduct = await _productRepository.GetProductByIdAsync(id);

                if (imageUrl == null)
                {
                    product.ImageUrl = existingProduct.ImageUrl;
                }
                else
                {
                    product.ImageUrl = await SaveImage(imageUrl);
                }
                existingProduct.ProductName = product.ProductName;
                existingProduct.ProductPrice = product.ProductPrice;
                existingProduct.Description = product.Description;
                existingProduct.CategoryId = product.CategoryId;
                existingProduct.ImageUrl = product.ImageUrl;
                await _productRepository.UpdateProductAsync(existingProduct);
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Categories = new SelectList(await _categoryRepository.GetAllCategoriesAsync(), "CategoryId", "CategoryName");
            return View(product);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var product = await _productRepository.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        [HttpPost, ActionName("DeleteConfirmed")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _productRepository.DeleteProductAsync(id);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Search(string keyword)
        {
            var products = string.IsNullOrEmpty(keyword)
                ? _context.Products.ToList()
                : _context.Products.Where(p => p.ProductName.Contains(keyword)).ToList();

            ViewBag.Keyword = keyword;
            return View(products);
        }
    }
}
