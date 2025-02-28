

namespace SellingBook.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        public string? ProductName { get; set; }
        public decimal ProductPrice { get; set; }
        public float? Rate { get; set; } = 0;
        public int? RateAmount { get; set; } = 0;
        public string? ImageUrl { get; set; }
        public List<String>? Images { get; set; }
        public string? Description { get; set; }
        public int CategoryId { get; set; }
        public virtual Category? Category { get; set; }
        public ICollection<CartItem>? CartItems { get; set; }
        public ICollection<OrderItem>? OrderItems { get; set; }

    }
}
