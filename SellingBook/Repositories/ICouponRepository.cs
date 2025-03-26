using SellingBook.Models.BasicModels;

namespace SellingBook.Repositories
{
    public interface ICouponRepository
    {
        decimal? ValidateAsync(string code);
    }
}
