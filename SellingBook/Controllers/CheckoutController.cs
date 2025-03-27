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

            if (response.VnPayResponseCode == "00")
            {
                ViewBag.OrderId = response.OrderId;
                return View("PaymentSucceed");
            }

            return View("PaymentFailed");
        }

        [HttpPost]
        public IActionResult SubmitComment(string orderId, string comment, int productId, string userId)
        {
            var savedComment = _orderRepository.SaveComment(orderId, comment, productId, userId);
            if (savedComment != null)
            {
                // Bình luận đã được lưu thành công, có thể sử dụng savedComment ở đây
                ViewBag.Comment = savedComment.Comment;
                ViewBag.OrderId = savedComment.OrderId;
                ViewBag.ProductId = savedComment.ProductId;
                ViewBag.UserId = savedComment.UserId;
                ViewBag.CreatedAt = savedComment.CreatedAt;
                return View("CommentSuccess");
            }

            // Nếu không tìm thấy đơn hàng, thông báo lỗi
            ViewBag.Message = "Order not found.";
            return View("Error");
        }
    }
}
