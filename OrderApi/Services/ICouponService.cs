using OrderApi.Dtos;

namespace OrderApi.Services
{
    public interface ICouponService
    {
        Task<CouponResponseDto> ApplyCouponAsync(string code, decimal cartTotal);
        Task<CouponResponseDto> GetByCodeAsync(string code);
    }
}
