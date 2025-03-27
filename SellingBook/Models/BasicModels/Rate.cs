using SellingBook.Models.Identity;

namespace SellingBook.Models.BasicModels
{
    public class Rate
    {
        public int RateId { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int RateAmount { get; set; }
        public string? Comment { get; set; }
    }
}
