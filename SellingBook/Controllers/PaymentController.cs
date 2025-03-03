using Microsoft.AspNetCore.Mvc;
using SellingBook.Services;
using System.Text.Json;

namespace SellingBook.Controllers
{
    public class PaymentController : Controller
    {
        private ILogger<PaymentController> _logger;
        private IMomoPaymentService _momoPaymentService;
        public PaymentController(ILogger<PaymentController> logger, IMomoPaymentService momoPaymentService)
        {
            _logger = logger;
            _momoPaymentService = momoPaymentService;
        }

        [HttpPost]
        public async Task<IActionResult> ReceivePaymentProcess()
        {
            var partnerCode = "abc";
            var body = Request.Body;
            var bodyReadeingStream = new StreamReader(body);
            var bodyContent = await bodyReadeingStream.ReadToEndAsync();
            var bodyJson = JsonSerializer.Deserialize<Dictionary<string, string>>(bodyContent);
            var amount = long.Parse(bodyJson["amount"]);

            var response = await _momoPaymentService.SendPaymentRequest(partnerCode, amount);
            var jsonResponse = JsonSerializer.Deserialize<Dictionary<string, string>>(response);

            if(jsonResponse["resultCode"] != "0" || string.IsNullOrEmpty(jsonResponse["payUrl"]))
            {
                _logger.LogError("Yêu cầu lên momo không hợp lệ.");
                return BadRequest();
            }

            return Redirect(jsonResponse["payUrl"]);
        }

        [HttpPost]
        public async Task<IActionResult> ResultEndpoint()
        {
            var body = Request.Body;
            var bodyReadeingStream = new StreamReader(body);
            var bodyContent = await bodyReadeingStream.ReadToEndAsync();
            var bodyJson = JsonSerializer.Deserialize<Dictionary<string, string>>(bodyContent);
            if (bodyJson["resultCode"] != "0")
            {
                _logger.LogError("Thanh toán không thành công.");
                return BadRequest();
            }
            _logger.LogInformation("Thanh toán thành công.");
            return Ok();
        }
    }
}
