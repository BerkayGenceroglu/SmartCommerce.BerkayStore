using Microsoft.EntityFrameworkCore;
using OrderApi.Context;
using OrderApi.Dtos;
using OrderApi.Entities;

namespace OrderApi.Services;

public class CouponService : ICouponService
{
    private readonly OrderDbContext _context;

    public CouponService(OrderDbContext context)
    {
        _context = context;
    }

    public async Task<CouponResponseDto> ApplyCouponAsync(string code, decimal cartTotal)
    {
        var coupon = await _context.Coupons
            .FirstOrDefaultAsync(x => x.Code.ToUpper() == code.ToUpper());

        if (coupon == null)
            throw new Exception("Kupon kodu bulunamadı!");

        if (!coupon.IsActive)
            throw new Exception("Bu kupon artık geçerli değil!");

        if (coupon.ExpiresAt < DateTime.UtcNow)
            throw new Exception("Kupon süresinin dolmuş!");

        if (coupon.UsageCount >= coupon.UsageLimit)
            throw new Exception("Kupon kullanım limiti dolmuş!");

        if (cartTotal < coupon.MinimumAmount)
            throw new Exception($"Bu kuponu kullanmak için minimum {coupon.MinimumAmount.ToString("N0")}₺ tutarında alışveriş yapmalısınız!");

        decimal discountedAmount = 0;

        if (coupon.DiscountType == DiscountType.Percentage)
            discountedAmount = cartTotal * coupon.DiscountValue / 100;
        else
            discountedAmount = coupon.DiscountValue;

        // İndirim sepet tutarından fazla olamaz
        if (discountedAmount > cartTotal)
            discountedAmount = cartTotal;

        return new CouponResponseDto
        {
            Id = coupon.Id,
            Code = coupon.Code,
            DiscountType = coupon.DiscountType,
            DiscountValue = coupon.DiscountValue,
            MinimumAmount = coupon.MinimumAmount,
            DiscountedAmount = discountedAmount,
            FinalAmount = cartTotal - discountedAmount
        };
    }


    public async Task<CouponResponseDto> CreateAsync(CreateCouponDto dto)
    {
        var existing = await _context.Coupons
            .FirstOrDefaultAsync(x => x.Code.ToUpper() == dto.Code.ToUpper());

        if (existing != null)
            throw new Exception("Bu kupon kodu zaten mevcut!");

        var coupon = new Coupon
        {
            Code = dto.Code.ToUpper(),
            DiscountType = dto.DiscountType,
            DiscountValue = dto.DiscountValue,
            MinimumAmount = dto.MinimumAmount,
            UsageLimit = dto.UsageLimit,
            ExpiresAt = dto.ExpiresAt,
            IsActive = true,
            UsageCount = 0
        };

        await _context.Coupons.AddAsync(coupon);
        await _context.SaveChangesAsync();

        return new CouponResponseDto
        {
            Id = coupon.Id,
            Code = coupon.Code,
            DiscountType = coupon.DiscountType,
            DiscountValue = coupon.DiscountValue,
            MinimumAmount = coupon.MinimumAmount,
            UsageLimit = coupon.UsageLimit,
            UsageCount = coupon.UsageCount,
            ExpiresAt = coupon.ExpiresAt,
            IsActive = coupon.IsActive
        };
    }

    public async Task<List<CouponResponseDto>> GetAllAsync()
    {
        var coupons = await _context.Coupons
            .Select(c => new CouponResponseDto
            {
                Id = c.Id,
                Code = c.Code,
                DiscountType = c.DiscountType,
                DiscountValue = c.DiscountValue,
                ExpiresAt = c.ExpiresAt,      // ← eksikti
                UsageLimit = c.UsageLimit,    // ← eksikti
                UsageCount = c.UsageCount,    // ← eksikti
                IsActive = c.IsActive,         // ← eksikti
                MinimumAmount = c.MinimumAmount
            })
            .ToListAsync();
        return coupons;
    }

    public async Task<CouponResponseDto> GetByCodeAsync(string code)
    {
        var coupon = await _context.Coupons
            .FirstOrDefaultAsync(x => x.Code.ToUpper() == code.ToUpper());

        if (coupon == null)
            throw new Exception("Kupon kodu bulunamadı!");

        return new CouponResponseDto
        {
            Id = coupon.Id,
            Code = coupon.Code,
            DiscountType = coupon.DiscountType,
            DiscountValue = coupon.DiscountValue,
            MinimumAmount = coupon.MinimumAmount
        };
    }
}