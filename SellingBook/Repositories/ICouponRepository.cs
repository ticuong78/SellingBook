using SellingBook.Models.BasicModels;

namespace SellingBook.Repositories
{
    public interface ICouponRepository
    {
        Task<decimal?> ValidateAsync(string code);
    }
}
