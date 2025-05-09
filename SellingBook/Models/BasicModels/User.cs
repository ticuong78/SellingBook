﻿namespace SellingBook.Models.BasicModels
{
    [Obsolete("Use ApplicationUser model from Identity instead.")]
    public class User
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string UserPassword { get; set; }
        public ICollection<CartItem> CartItems { get; set; }
        public ICollection<Order> Orders { get; set; }
    }
}
