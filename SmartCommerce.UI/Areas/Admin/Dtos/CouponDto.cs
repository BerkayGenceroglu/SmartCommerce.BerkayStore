namespace SmartCommerce.UI.Areas.Admin.Dtos
{
    public class CouponDto
    {
        public Guid Id { get; set; }
        public string Code { get; set; } = null!;
        public int DiscountType { get; set; }
        public decimal DiscountValue { get; set; }
        public decimal? MinOrderAmount { get; set; }
        public int UsageCount { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
