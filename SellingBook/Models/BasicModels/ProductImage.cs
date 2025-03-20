namespace SellingBook.Models.BasicModels
{
    public class ProductImage
    {
        public int ProductImageId { get; set; }
        public string ProductImageUrl { get; set; }
        public int ProductId { get; set; }
        public Product? Product { get; set; }
    }
}
