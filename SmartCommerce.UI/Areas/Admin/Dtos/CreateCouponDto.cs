namespace SmartCommerce.UI.Areas.Admin.Dtos
{
    public class CreateCouponDto
    {
        public string Code { get; set; } = null!;
        public int DiscountType { get; set; }
        public decimal DiscountValue { get; set; }
        public decimal? MinOrderAmount { get; set; }
    }
}
