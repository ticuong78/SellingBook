using System;
using System.ComponentModel.DataAnnotations;

namespace SellingBook.Models.BasicModels
{
    public class Coupon
    {
        [Key]
        public int CouponId { get; set; }

        [Required]
        [StringLength(50)]
        public string Code { get; set; } // Mã giảm giá

        [Required]
        [Range(0, double.MaxValue)]
        public decimal DiscountValue { get; set; } // Số tiền giảm cố định

        public bool IsActive { get; set; } = true; // Mã có đang hoạt động không
    }
}
