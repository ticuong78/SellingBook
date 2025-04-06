using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SellingBook.Repositories;

namespace SellingBook.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class CouponController : Controller
    {
        private readonly ICouponRepository _couponRepository;

        public CouponController(ICouponRepository couponRepository)
        {
            _couponRepository = couponRepository;
        }

        [HttpPost("ValidateCode")]
        public async Task<IActionResult> ValidateCode([FromBody] string code)
        {
            Console.WriteLine($"ValidateCode - Received Code: {code}");

            var discountValue = await _couponRepository.ValidateAsync(code);

            if (discountValue.HasValue)
            {
                Console.WriteLine($"ValidateCode - Discount: {discountValue.Value}");
                return Json(new { discountValue });
            }
            else
            {
                Console.WriteLine("ValidateCode - Invalid Code!");
                return NotFound(new { message = "Mã giảm giá không hợp lệ!" });
            }
        }
        [HttpGet("GetCoupons")]
        public async Task<IActionResult> GetCoupons()
        {
            try
            {
                var coupons = await _couponRepository.GetAllCoupons();  // Lấy tất cả mã giảm giá từ CSDL
                var result = coupons.Select(c => new { c.Code, c.DiscountValue }).ToList();
                return Json(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi khi lấy danh sách mã giảm giá." });
            }
        }
    }
}
