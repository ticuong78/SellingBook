using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SellingBook.Models.BasicModels
{
    public class CartItem
    {
        [Key]
        [Column("Id")]
        public int CartItemId { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int CartItemQuantity { get; set; }
        public decimal CartItemPrice { get; set; }
    }
}
