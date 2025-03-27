using SellingBook.Models.Identity;

namespace SellingBook.Models.BasicModels
{
    public class Order
    {
        public string OrderId { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public string OrderAddress { get; set; }
        public string OrderPhone { get; set; }
        public string OrderEmail { get; set; }
        public string OrderStatus { get; set; }
        public DateTime CreatedAt { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; }
        public int? CouponId { get; set; } // Có thể không có mã giảm giá
        public Coupon Coupon { get; set; }

        // Tổng tiền sau khi áp dụng mã giảm giá
        public decimal TotalAmount { get; set; }
    }
}
