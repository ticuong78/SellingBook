﻿using SellingBook.Models;

namespace SellingBook.Repositories
{
    public interface ICartRepository
    {
        int GetCartItemsCountBasedOnIds();
        int GetCartItemsCountBasedOnRealTotal();
        void AddCartItem(CartItem cartItem);
        void DeleteCartItem(CartItem cartItem);
<<<<<<< HEAD
=======
        IEnumerable<CartItem> GetCartItems();
>>>>>>> feature/payment-methods-momo
    }
}
