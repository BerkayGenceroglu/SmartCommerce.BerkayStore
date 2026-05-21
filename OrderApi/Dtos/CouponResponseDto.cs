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
    }
}
