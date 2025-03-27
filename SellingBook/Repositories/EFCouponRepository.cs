using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SellingBook.Models;
using SellingBook.Repositories;

public class EFCouponRepository : ICouponRepository
{
    private readonly ApplicationDbContext _context;

    public EFCouponRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<decimal?> ValidateAsync(string code)
    {
        if (string.IsNullOrWhiteSpace(code)) return null;

        code = code.Trim(); // Loại bỏ khoảng trắng thừa

        var findcoupon = await _context.Coupons
                                       .FirstOrDefaultAsync(c => c.Code == code && c.IsActive);

        return findcoupon?.DiscountValue;
    }


}
