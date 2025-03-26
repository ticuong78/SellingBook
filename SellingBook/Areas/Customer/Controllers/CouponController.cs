using Microsoft.AspNetCore.Mvc;
using SellingBook.Repositories;

namespace SellingBook.Areas.Customer.Controllers
{
    public class CouponController : Controller
    {
        private readonly ICouponRepository _couponRepository;
        public CouponController(ICouponRepository couponRepository)
        {
            _couponRepository = couponRepository;
        }

        [HttpPost]
        public IActionResult ValidateCode([FromBody] string code)
        {
            var discountValue = _couponRepository.ValidateAsync(code);

            if(discountValue != null)
            {
                return Json(discountValue);
            } else
            {
                return NotFound("Cannot not find any matched Coupon");
            }
        }
    }
}
