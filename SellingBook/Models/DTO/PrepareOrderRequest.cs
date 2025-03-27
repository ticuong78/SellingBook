using SellingBook.Models.BasicModels;

namespace SellingBook.Models.DTO
{
    public class PrepareOrderRequest
    {
        public List<CartItem> SelectedItems { get; set; }
        public int CouponId { get; set; }
    }
}
