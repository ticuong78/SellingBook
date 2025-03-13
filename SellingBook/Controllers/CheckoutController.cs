using Microsoft.AspNetCore.Mvc;
using SellingBook.Models.VNPay;
using SellingBook.Services.VNPay;
using System.Security.Claims;
using System.Text.Json;

namespace SellingBook.Controllers
{
    public class CheckoutController : Controller
    {

        private readonly IVNPayService _vnPayService;
        public CheckoutController(IVNPayService vnPayService)
        {

            _vnPayService = vnPayService;
        }

        public IActionResult CreatePaymentUrlVnPay(PaymentInformationModel model)
        {
            var url = _vnPayService.CreatePaymentUrl(model, HttpContext);

            return Redirect(url);
        }

        [HttpGet]
        public IActionResult PaymentCallBackVnPay()
        {
            var response = _vnPayService.PaymentExecute(Request.Query);

            return Json(response);
        }
    }
}
