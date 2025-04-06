using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SellingBook.Models;
using SellingBook.Models.BasicModels;
using SellingBook.Models.Roles;
using SellingBook.Repositories;
using System.Threading.Tasks;

namespace SellingBook.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = $"{SD.Role_Admin}")]
    [Route("/Admin/Coupon")]
    public class CouponController : Controller
    {
        private readonly ICouponRepository _couponRepository;

        public CouponController(ICouponRepository couponRepository)
        {
            _couponRepository = couponRepository;
        }

        // Danh sách mã giảm giá
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllCoupons()
        {
            var coupons = await _couponRepository.GetAllCoupons();
            return Json(coupons);
        }

        // Thêm mã giảm giá
        [HttpPost("Create")]
        public async Task<IActionResult> CreateCoupon([FromBody] Coupon model)
        {
            if (string.IsNullOrWhiteSpace(model.Code) || model.DiscountValue <= 0)
                return BadRequest(new { message = "Dữ liệu không hợp lệ!" });

            var result = await _couponRepository.AddCouponAsync(model);
            return result ? Ok(new { message = "Thêm thành công!" }) : StatusCode(500, new { message = "Lỗi khi thêm mã giảm giá!" });
        }

        // Cập nhật mã giảm giá
        [HttpPut("Update/{id}")]
        public async Task<IActionResult> UpdateCoupon(int id, [FromBody] Coupon model)
        {
            if (string.IsNullOrWhiteSpace(model.Code) || model.DiscountValue <= 0)
                return BadRequest(new { message = "Dữ liệu không hợp lệ!" });

            var result = await _couponRepository.UpdateCouponAsync(id, model);
            return result ? Ok(new { message = "Cập nhật thành công!" }) : NotFound(new { message = "Không tìm thấy mã giảm giá!" });
        }

        // Xóa mã giảm giá
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeleteCoupon(int id)
        {
            var result = await _couponRepository.DeleteCouponAsync(id);
            return result ? Ok(new { message = "Xóa thành công!" }) : NotFound(new { message = "Không tìm thấy mã giảm giá!" });
        }
        
        public IActionResult Index()
        {
            return View();
        }
    }
}
