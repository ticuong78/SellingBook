using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RestSharp;
using SellingBook.Models.Momo;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace SellingBook.Services
{
    public class MomoService: IMomoService
    {
        private static readonly HttpClient client = new HttpClient();
        private readonly IOptions<MomoOptionModel> _options;

        public MomoService(IOptions<MomoOptionModel> options)
        {
            _options = options;
        }

        public async Task<MomoCreatePaymentResponseModel> CreatePaymentAsync(int amount)
        {
            var OrderId = DateTime.UtcNow.Ticks.ToString(); // Unique long ID
            var RequestId = Guid.NewGuid().ToString("N").Substring(0, 15); // Shorter unique ID

            var OrderInfo = $"Khách hàng: Lê Phạm Hùng Cường. Nội dung: {OrderId}";
            var Amount = amount.ToString(); // Ensure it's a string

            //var rawSignature = "accessKey=" + _options.Value.AccessKey + "&amount=" + Amount + "&extraData=&orderId=" + OrderId + "&partnerCode=" + _options.Value.PartnerCode + "&paymentCode=" + _options.Value.PartnerCode + "&requestId=" + RequestId;
            // Construct rawData correctly
            var rawData = string.Join("&",
                $"partnerCode={_options.Value.PartnerCode}",
                $"accessKey={_options.Value.AccessKey}",
                $"requestId={RequestId}",
                $"amount={Amount}",
                $"orderId={OrderId}",
                $"orderInfo={OrderInfo}", // Fix encoding issue
                $"returnUrl={_options.Value.ReturnUrl}",
                $"notifyUrl={_options.Value.NotifyUrl}",
                $"extraData="
            );

            Console.WriteLine("RawData Before Hashing: " + rawData); // Debugging log

            // Compute correct HMAC SHA256 signature
            var signature = ComputeHmacSha256(rawData, _options.Value.SecretKey);

            // Create JSON payload
            var requestData = new
            {
                accessKey = _options.Value.AccessKey,
                partnerCode = _options.Value.PartnerCode,
                requestType = _options.Value.RequestType,
                notifyUrl = _options.Value.NotifyUrl,
                returnUrl = _options.Value.ReturnUrl,
                orderId = OrderId,
                amount = Amount,
                orderInfo = OrderInfo,
                requestId = RequestId,
                extraData = "",
                signature = signature
            };

            StringContent httpContent = new StringContent(JsonSerializer.Serialize(requestData), Encoding.UTF8, "application/json");
            var quickPayResponse = await client.PostAsync(_options.Value.MomoApiUrl, httpContent);
            var contents = quickPayResponse.Content.ReadAsStringAsync().Result;
            System.Console.WriteLine(contents + "");
            return JsonConvert.DeserializeObject<MomoCreatePaymentResponseModel>(contents);
        }

        // Hàm xử lý callback từ momo trả về
        public MomoExecuteResponseModel PaymentExecuteAsync(IQueryCollection collection)
        {
            var amount = collection.First(s => s.Key == "amount").Value;
            var orderInfo = "Thanh toán đơn hàng cho Lê Phạm Hùng Cường";
            var orderId = DateTime.UtcNow.Ticks.ToString();

            return new MomoExecuteResponseModel()
            {
                Amount = amount,
                OrderId = orderId,
                OrderInfo = orderInfo
            };
        }


        // hàm băm để tạo chữ ký cho momo
        private string ComputeHmacSha256(string message, string secretKey)
        {
            var keyBytes = Encoding.UTF8.GetBytes(secretKey);
            var messageBytes = Encoding.UTF8.GetBytes(message); // Ensure UTF-8 encoding

            using (var hmac = new HMACSHA256(keyBytes))
            {
                return BitConverter.ToString(hmac.ComputeHash(messageBytes)).Replace("-", "").ToLower();
            }
        }
    }
}
