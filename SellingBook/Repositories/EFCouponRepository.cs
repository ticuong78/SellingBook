using SellingBook.Models;
using SellingBook.Models.BasicModels;

namespace SellingBook.Repositories
{
    public class EFCouponRepository : ICouponRepository
    {
        private readonly ApplicationDbContext _context;
        public EFCouponRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public decimal? ValidateAsync(string code)
        {
            var findcoupon = _context.Coupons.FindAsync(code).Result;

            if (findcoupon == null)
                return null;
            else
                return findcoupon.DiscountValue;
        }
    }
}
