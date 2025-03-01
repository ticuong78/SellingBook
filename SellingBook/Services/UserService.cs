using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SellingBook.Models;

namespace SellingBook.Services
{
    public class UserService: IUserService
    {
        private ApplicationDbContext _applicationDbContext;
        private ILogger<UserService> _logger;

        public UserService(ApplicationDbContext applicationDbContext, ILogger<UserService> logger) 
        {
            _applicationDbContext = applicationDbContext;
            _logger = logger;
        }

        public User GetUser()
        {
            return _applicationDbContext.Users.FirstOrDefault(user => user.UserId == 1);
        }

        public void AddCartItem(CartItem cartItem)
        {
            // Update cart items
            _logger.LogInformation(cartItem.UserId.ToString());

            CartItem existingCartItem = _applicationDbContext.CartItems.FirstOrDefault(item => item.ProductId == cartItem.ProductId);
            
            if(existingCartItem != null)
            {
                existingCartItem.CartItemQuantity += cartItem.CartItemQuantity;
                existingCartItem.CartItemPrice += cartItem.CartItemPrice;
                _applicationDbContext.SaveChanges();
                return;
            }

            _applicationDbContext.CartItems.Add(cartItem);
            _applicationDbContext.SaveChanges();
        }

        public void DeleteCartItem(CartItem cartItem)
        {
            // Update cart items
            _applicationDbContext.CartItems.Remove(cartItem);
            _applicationDbContext.SaveChanges();
        }

        public int GetCartItemsCount()
        {
            return _applicationDbContext.CartItems.Where(cartItem => cartItem.UserId == 1).Count();
        }
    }
}
