using SmartCommerce.UI.Areas.Admin.Dtos;

namespace SmartCommerce.UI.Areas.Admin.Abstract
{
    public interface ICouponService
    {
        Task<List<CouponDto>> GetAllAsync();
        Task<bool> CreateAsync(CreateCouponDto dto, string token);
        Task<bool> DeleteAsync(Guid id, string token);
    }
}
