using SellingBook.Models.BasicModels;

namespace SellingBook.Models.Checkout
{
    public class ProductCartItemViewModel
    {
        public IEnumerable<Product> Products { get; set; }
        public IEnumerable<CartItem> CartItems { get; set; }
        public ProductCartItemViewModel(){}
    }
}
