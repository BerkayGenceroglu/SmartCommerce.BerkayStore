namespace OrderApi.Dtos
{
    public class CartResponseDto
    {
        public Guid UserId { get; set; }
        public List<CartItemDto> Items { get; set; } = new();
        public decimal TotalAmount => Items.Sum(x => x.UnitPrice * x.Quantity);
        public DateTime ExpiresAt { get; set; }
    }
    public class CartItemDto
    {
        public Guid ProductId { get; set; }
        public string ProductName { get; set; } = null!;
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public string? VariantInfo { get; set; }
    }
}
