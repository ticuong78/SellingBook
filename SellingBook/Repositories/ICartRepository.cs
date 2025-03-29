using SellingBook.Models.BasicModels;

namespace SellingBook.Repositories
{
    public interface ICartRepository
    {
        // Lấy tổng số lượng sản phẩm trong giỏ hàng
        int GetCartItemsCountBasedOnIds();

        // Lấy tổng số lượng sản phẩm trong giỏ hàng theo thực tế
        int GetCartItemsCountBasedOnRealTotal();

        // Thêm một CartItem vào giỏ hàng
        Task AddCartItem(CartItem cartItem);

        // Xóa CartItem khỏi giỏ hàng
        Task DeleteCartItem(CartItem cartItem);

        // Lấy danh sách CartItem của người dùng hiện tại
        IEnumerable<CartItem> GetCartItems();

        // Lưu các thay đổi vào cơ sở dữ liệu
        Task SaveChangesAsync();
    }
}
