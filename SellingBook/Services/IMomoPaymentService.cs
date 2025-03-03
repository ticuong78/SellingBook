namespace SellingBook.Services
{
    public interface IMomoPaymentService
    {
        Task<string> SendPaymentRequest(string partnerCode, long amount);
    }
}
