using SellingBook.Models.BasicModels;

namespace SellingBook.Models.DTO
{
    public class ProductCartItemViewModel
    {
        public IEnumerable<Product> Products { get; set; }
        public IEnumerable<CartItem> CartItems { get; set; }
        public ProductCartItemViewModel(){}
    }
}
