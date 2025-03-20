using Microsoft.AspNetCore.Mvc;
using SellingBook.Models.VNPay;
using SellingBook.Repositories;
using SellingBook.Services.VNPay;

namespace SellingBook.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly IVNPayService _vnPayService;
        private readonly IOrderRepository _orderRepository;

        public CheckoutController(IVNPayService vnPayService, IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
            _vnPayService = vnPayService;
        }

        public IActionResult CreatePaymentUrlVnPay([FromBody] PaymentInformationModel model)
        {
            var url = _vnPayService.CreatePaymentUrl(model, HttpContext);

            return Json(new
            {
                paymentUrl = url
            });
        }

        [HttpGet]
        public IActionResult PaymentCallBackVnPay()
        {
            var response = _vnPayService.PaymentExecute(Request.Query);

            if (response.Success)
            {
                ViewBag.OrderId = response.OrderId;
                return View("PaymentSucceed");
            }

            return View("PaymentFailed");
        }
    }
}
