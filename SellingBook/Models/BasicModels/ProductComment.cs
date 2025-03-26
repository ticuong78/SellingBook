using SellingBook.Models.BasicModels;

namespace SellingBook.Models
{
    public class ProductComment
    {
        public Guid Id { get; set; }
        public string OrderId { get; set; }   // Khóa ngoại
        public int ProductId { get; set; }     // Khóa ngoại
        public string UserId { get; set; }     // Khóa ngoại
        public string Comment { get; set; }
        public DateTime CreatedAt { get; set; }

        // Các quan hệ với các bảng khác
        public Order Order { get; set; }  // Liên kết với bảng Order
        public Product Product { get; set; }  // Liên kết với bảng Product
        public User User { get; set; }    // Liên kết với bảng User
    }
}
