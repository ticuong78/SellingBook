using Microsoft.AspNetCore.Mvc;
using SellingBook.Services;
using System.Security.Claims;
using System.Text.Json;

namespace SellingBook.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly ILogger<CheckoutController> _logger;
        private readonly IMomoService _momoService;
        public CheckoutController(IMomoService momoService, ILogger<CheckoutController> logger)
        {
            _momoService = momoService;
            _logger = logger;
        }

        [HttpPost]
        public IActionResult CreatePayment(int amount)
        {
            var response = _momoService.CreatePaymentAsync(amount).Result;

            if(response.PayUrl == null)
            {
                return BadRequest(JsonSerializer.Serialize(response));
            }

            return Redirect(response.PayUrl);
        }
        [HttpGet]
        public IActionResult PaymentCallBack() // Momo payment callback, xử lý kết quả trả về từ Momo 
        {
            var response = _momoService.PaymentExecuteAsync(HttpContext.Request.Query);
            var requestQuery = HttpContext.Request.Query;
            if (requestQuery["resultCode"] == 0)
            {
                _logger.LogInformation("Momo payment success"); // Log nếu giao dịch thành công
                TempData["success"] = "Giao dịch Momo thành công.";
                return RedirectToAction("Index", "Cart");
            }
            else
            {
                _logger.LogInformation("Momo payment failed"); // Log nếu giao dịch thất bại
                TempData["success"] = "Đã hủy giao dịch Momo.";
                return RedirectToAction("Index", "Cart");
            }
        }
    }
}
