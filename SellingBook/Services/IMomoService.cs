using SellingBook.Models.Momo;

namespace SellingBook.Services
{
    public interface IMomoService
    {
        Task<MomoCreatePaymentResponseModel> CreatePaymentAsync(int amount);
        MomoExecuteResponseModel PaymentExecuteAsync(IQueryCollection collection);
    }
}
