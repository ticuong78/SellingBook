using Microsoft.AspNetCore.Mvc;
using SellingBook.Repositories;

namespace SellingBook.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Route("Customer/Coupon")]
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
    }
}
