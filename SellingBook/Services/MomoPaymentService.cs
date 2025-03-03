using System.Text;
using System.Text.Json;

namespace SellingBook.Services
{
    public class MomoPaymentService: IMomoPaymentService
    {
        private readonly HttpClient _httpClient = new();

        public async Task<string> SendPaymentRequest(string partnerCode, long amount)
        {
            string requestId = Guid.NewGuid().ToString("N").Substring(0, 16);
            string orderId = new Guid(string.Format($"{requestId}-{amount}-{DateTime.UtcNow.Ticks}")).ToString();

            var requestUrl = "https://test-payment.momo.vn/v2/gateway/api/create";
            var requestData = new
            {
                partnerCode,
                requestId,
                amount,
                orderId,
                orderInfo = $"Thanh toán cho đơn hàng có mã {orderId}",
                redirectUrl = "http://localhost:7080/",
                ipnUrl = "http://localhost:7080/Payment/ResultEndpoint",
                extraData = "",
                requestType = "captureWallet",
                lang = "vi",
                signature = "LeCuong1709",
            };

            var json = JsonSerializer.Serialize(requestData);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(requestUrl, content);
            var responseContent = await response.Content.ReadAsStringAsync();
            return responseContent;
        }
    }
}
