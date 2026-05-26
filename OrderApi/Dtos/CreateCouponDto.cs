using OrderApi.Entities;

namespace OrderApi.Dtos
{
    public class CreateCouponDto
    {
        public string Code { get; set; } = null!;
        public DiscountType DiscountType { get; set; }
        public decimal DiscountValue { get; set; }
        public decimal MinimumAmount { get; set; }
        public int UsageLimit { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}
