
﻿using SellingBook.Models;
namespace SellingBook.Models
{
    public class Category
    {
        
        public int CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public ICollection<Product>? Products { get; set; }

    }
}
