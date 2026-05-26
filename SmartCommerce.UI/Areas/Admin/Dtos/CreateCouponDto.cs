namespace SmartCommerce.UI.Areas.Admin.Dtos
{
    public class CreateCouponDto
    {
        public string Code { get; set; } = null!;
        public int DiscountType { get; set; }
        public decimal DiscountValue { get; set; }
        public decimal MinimumAmount { get; set; }   // MinOrderAmount → MinimumAmount
        public int UsageLimit { get; set; }           // ekle
        public DateTime ExpiresAt { get; set; }     // ekle
    }
}
