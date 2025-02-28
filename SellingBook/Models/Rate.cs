namespace SellingBook.Models
{
    public class Rate
    {
        public int RateId { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int RateAmount { get; set; }
        public string? Comment { get; set; }
    }
}
