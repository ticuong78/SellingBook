using SellingBook.Models;

namespace SellingBook.Services
{
    public interface IUserService
    {
        User GetUser();
        void AddCartItem(CartItem cartItem);
        void DeleteCartItem(CartItem cartItem);
        int GetCartItemsCount();
    }
}
