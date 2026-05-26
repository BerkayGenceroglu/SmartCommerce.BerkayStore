namespace SmartCommerce.UI.Areas.Admin.Dtos
{
    public class CouponDto
    {
        public Guid Id { get; set; }
        public string Code { get; set; } = null!;
        public int DiscountType { get; set; }
        public decimal DiscountValue { get; set; }
        public decimal MinimumAmount { get; set; }  // MinOrderAmount → MinimumAmount
        public int UsageLimit { get; set; }          // ekle
        public int UsageCount { get; set; }
        public bool IsActive { get; set; }
        public DateTime ExpiresAt { get; set; }      // ekle
        public DateTime CreatedAt { get; set; }
    }
}
