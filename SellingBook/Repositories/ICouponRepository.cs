using SellingBook.Models;
using SellingBook.Models.BasicModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SellingBook.Repositories
{
    public interface ICouponRepository
    {
        Task<decimal?> ValidateAsync(string code);
        Task<List<Coupon>> GetAllCoupons();
        Task<bool> AddCouponAsync(Coupon coupon);
        Task<bool> UpdateCouponAsync(int id, Coupon coupon);
        Task<bool> DeleteCouponAsync(int id);
    }
}
