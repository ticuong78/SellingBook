using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SellingBook.Extensions;
using SellingBook.Models.BasicModels;
using SellingBook.Models.DTO;
using SellingBook.Models.VNPay;
using SellingBook.Repositories;
using SellingBook.Services;
using SellingBook.Services.VNPay;
using System.Text.Json;

namespace SellingBook.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly IVNPayService _vnPayService;

        public CheckoutController(IVNPayService vnPayService, IOrderService orderService)
        {
            _vnPayService = vnPayService;
            _orderService = orderService;
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
                _orderService.AddOrder(response.OrderId, response.OrderDescription, "CurrentCouponId", "CurrentOrderItems", HttpContext.Session);
                return View("PaymentSucceed");
            }

            return View("PaymentFailed");
        }

        [HttpPost]
        public IActionResult PrepareForOrder([FromBody] PrepareOrderRequest request)
        {
            try
            {
                HttpContext.Session.SetObjectAsJson("CurrentOrderItems", request.SelectedItems);
                HttpContext.Session.SetInt32("CurrentCouponId", request.CouponId);

                return Ok();
            }
            catch (Exception e)
            {
                return Json(new
                {
                    message = e.Message
                });
            }
        }

        [HttpPost]
        public IActionResult SubmitComment(string orderId, string comment, int productId, string userId)
        {
            var savedComment = _orderService.SaveComment(orderId, comment, productId, userId);
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
