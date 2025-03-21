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
    }
}
