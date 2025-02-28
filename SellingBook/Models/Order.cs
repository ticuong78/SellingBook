namespace SellingBook.Models
{
    public class Order
    {

        public int OrderId { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public string OrderAddress { get; set; }
        public string OrderPhone { get; set; }
        public string OrderEmail { get; set; }
        public string OrderStatus { get; set; }
        public DateTime CreatedAt { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; }
    }
}
