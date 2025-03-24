using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using SellingBook.Models.BasicModels;
using SellingBook.Models.Roles;
using SellingBook.Repositories;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace SellingBook.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = $"{SD.Role_Admin},{SD.Role_Employee},{SD.Role_Customer}")]
    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;

        public ProductController(
            IProductRepository productRepository,
            ICategoryRepository categoryRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
        }

        public async Task<IActionResult> Index() // display all products
        {
            var products = await _productRepository.GetAllProductsAsync();
            return View(products);
        }

        public async Task<IActionResult> Display(int id) // display a single product
        {
            var product = await _productRepository.GetProductByIdAsync(id);
            return View(product);
        }

        public async Task<IActionResult> Add()
        {
            if (User.IsInRole("Customer"))
            {
                return Redirect("/Identity/Account/AccessDenied");

            }

            var categories = await _categoryRepository.GetAllCategoriesAsync();
            ViewBag.Categories = new SelectList(categories, "CategoryId", "CategoryName");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddConfirmed(Product product, IFormFile imageURL, List<IFormFile> imageURLs)
        {
            if (User.IsInRole("Customer"))
            {
                return Redirect("/Identity/Account/AccessDenied");

            }

            if (ModelState.IsValid)
            {
                if (imageURL != null)
                {
                    product.ImageUrl = await AddImage(imageURL);
                }

                if (imageURLs != null)
                {
                    product.Images = new List<string>();
                    foreach (var image in imageURLs)
                    {
                        product.Images.Add(AddImage(image).Result);
                    }
                }

                await _productRepository.AddProductAsync(product);
                return RedirectToAction("Index");
            }
            return View("Add", product);
        }

        public async Task<string> AddImage(IFormFile image)
        {
            if (image != null)
            {
                var path = Path.Combine("wwwroot", "images", image.FileName);
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await image.CopyToAsync(stream);
                }
            }
            return "/images/" + image.FileName;
        }

        public async Task<IActionResult> Edit(int id)
        {
            if (User.IsInRole("Customer"))
            {
                return Redirect("/Identity/Account/AccessDenied");

            }

            var product = await _productRepository.GetProductByIdAsync(id);
            ViewBag.Categories = new SelectList(await _categoryRepository.GetAllCategoriesAsync(), "CategoryId", "CategoryName");

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> EditConfirmed(Product product, IFormFile imageURl)
        {
            if (User.IsInRole("Customer"))
            {
                return Redirect("/Identity/Account/AccessDenied");

            }

            ModelState.Remove("ImageURL");
            if (ModelState.IsValid)
            {
                await _productRepository.UpdateProductAsync(product);

                if (imageURl != null)
                {
                    product.ImageUrl = await AddImage(imageURl);
                    await _productRepository.UpdateProductAsync(product);
                }

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

        [HttpPost, ActionName("DeleteConfirmed")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _productRepository.DeleteProductByIdAsync(id);
            return RedirectToAction(nameof(Index));
        }

    }
}
