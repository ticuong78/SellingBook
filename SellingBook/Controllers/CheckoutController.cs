using Microsoft.AspNetCore.Mvc;
using SellingBook.Services;
using System.Text.Json;

namespace SellingBook.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly IMomoService _momoService;
        public CheckoutController(IMomoService momoService)
        {
            _momoService = momoService;
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
        public IActionResult PaymentExecute()
        {
            var response = _momoService.PaymentExecuteAsync(HttpContext.Request.Query);
            return View(response);
        }
    }
}
