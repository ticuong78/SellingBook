namespace SellingBook.Models.BasicModels
{
    public class OrderItem
    {
        public string OrderItemId { get; set; }
        public string OrderId { get; set; }
        public Order Order { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int OrderItemQuantity { get; set; }
        public decimal OrderItemPrice { get; set; }
    }
}
