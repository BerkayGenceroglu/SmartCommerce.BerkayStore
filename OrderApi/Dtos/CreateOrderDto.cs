namespace OrderApi.Dtos
{
    public class CreateOrderDto
    {
        public string Address { get; set; } = null!;
        public string? CouponCode { get; set; }
    }
}
