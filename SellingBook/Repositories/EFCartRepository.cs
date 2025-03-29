using SellingBook.Models.BasicModels;
using SellingBook.Models.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq;
using SellingBook.Models;
using SellingBook.Services.User;

namespace SellingBook.Repositories
{
    public class EFCartRepository : ICartRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly ILogger<EFCartRepository> _logger;
        private readonly IUserService _userService;

        public EFCartRepository(ApplicationDbContext applicationDbContext, ILogger<EFCartRepository> logger, IUserService userService)
        {
            _applicationDbContext = applicationDbContext;
            _logger = logger;
            _userService = userService;
        }

        // Thêm CartItem vào giỏ hàng
        public async Task AddCartItem(CartItem cartItem)
        {
            var user = _applicationDbContext.Users.FirstOrDefault(u => u.Email == _userService.GetCurrentUserEmail());
            var product = await _applicationDbContext.Products.FirstOrDefaultAsync(p => p.ProductId == cartItem.ProductId);

            // Kiểm tra nếu CartItem đã tồn tại trong giỏ hàng của người dùng
            var existingCartItem = await _applicationDbContext.CartItems
                .FirstOrDefaultAsync(item => item.ProductId == cartItem.ProductId && item.UserId == user.Id);

            if (existingCartItem != null)
            {
                existingCartItem.CartItemQuantity += cartItem.CartItemQuantity;
                existingCartItem.CartItemPrice += cartItem.CartItemPrice;
                await SaveChangesAsync(); // Cập nhật giỏ hàng
                return;
            }

            if (cartItem.Product == null)
            {
                cartItem.Product = product;
            }

            if (cartItem.User == null)
            {
                cartItem.User = user;
                cartItem.UserId = user.Id;
            }

            // Thêm CartItem mới vào cơ sở dữ liệu
            _applicationDbContext.CartItems.Add(cartItem);
            await SaveChangesAsync();
        }

        // Lấy tất cả CartItems của người dùng hiện tại
        public IEnumerable<CartItem> GetCartItems()
        {
            return _applicationDbContext.CartItems
                .Where(cartItem => cartItem.User.Email == _userService.GetCurrentUserEmail())
                .Include(ci => ci.Product); // Đảm bảo lấy thông tin sản phẩm liên quan
        }

        // Đếm số lượng CartItems trong giỏ hàng
        public int GetCartItemsCountBasedOnIds()
        {
            return _applicationDbContext.CartItems
                .Where(cartItem => cartItem.User.Email == _userService.GetCurrentUserEmail())
                .Count();
        }

        // Tính tổng số lượng các CartItems trong giỏ hàng
        public int GetCartItemsCountBasedOnRealTotal()
        {
            return _applicationDbContext.CartItems
                .Where(cartItem => cartItem.User.Email == _userService.GetCurrentUserEmail())
                .Sum(cart => cart.CartItemQuantity);
        }

        // Xóa CartItem khỏi cơ sở dữ liệu
        public async Task DeleteCartItem(CartItem cartItem)
        {
            _applicationDbContext.CartItems.Remove(cartItem);
            await SaveChangesAsync();
        }

        // Lưu các thay đổi vào cơ sở dữ liệu
        public async Task SaveChangesAsync()
        {
            await _applicationDbContext.SaveChangesAsync();
        }
    }
}
