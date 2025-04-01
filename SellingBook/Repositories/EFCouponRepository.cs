using Microsoft.EntityFrameworkCore;
using SellingBook.Models;
using SellingBook.Models.BasicModels;
using SellingBook.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class EFCouponRepository : ICouponRepository
{
    private readonly ApplicationDbContext _context;

    public EFCouponRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<decimal?> ValidateAsync(string code)
    {
        var coupon = await _context.Coupons
            .FirstOrDefaultAsync(c => c.Code == code && c.IsActive);
        return coupon?.DiscountValue;
    }

    public async Task<List<Coupon>> GetAllCoupons()
    {
        return await _context.Coupons.ToListAsync();
    }

    public async Task<bool> AddCouponAsync(Coupon coupon)
    {
        if (coupon == null) return false;

        _context.Coupons.Add(coupon);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> UpdateCouponAsync(int id, Coupon updatedCoupon)
    {
        var coupon = await _context.Coupons.FindAsync(id);
        if (coupon == null) return false;

        coupon.Code = updatedCoupon.Code;
        coupon.DiscountValue = updatedCoupon.DiscountValue;
        coupon.IsActive = updatedCoupon.IsActive;

        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteCouponAsync(int id)
    {
        var coupon = await _context.Coupons.FindAsync(id);
        if (coupon == null) return false;

        _context.Coupons.Remove(coupon);
        return await _context.SaveChangesAsync() > 0;
    }
}
