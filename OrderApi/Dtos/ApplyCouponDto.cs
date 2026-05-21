namespace OrderApi.Dtos
{
    public class ApplyCouponDto
    {
        public string Code { get; set; } = null!;
        public decimal CartTotal { get; set; }
    }
}
