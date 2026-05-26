using OrderApi.Entities;

namespace OrderApi.Dtos
{
    public class CouponResponseDto
    {
        public Guid Id { get; set; }
        public string Code { get; set; } = null!;
        public DiscountType DiscountType { get; set; }
        public decimal DiscountValue { get; set; }
        public decimal MinimumAmount { get; set; }
        public decimal DiscountedAmount { get; set; }
        public decimal FinalAmount { get; set; }
        public DateTime ExpiresAt { get; set; }  // ← ekle
        public int UsageLimit { get; set; }       // ← ekle
        public int UsageCount { get; set; }       // ← ekle
        public bool IsActive { get; set; }
    }
}
