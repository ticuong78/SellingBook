using SellingBook.Models.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SellingBook.Models.BasicModels
{
    public class CartItem
    {
        [Key]
        [Column("Id")]
        public int CartItemId { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int CartItemQuantity { get; set; }
        public decimal CartItemPrice { get; set; }
    }
}
