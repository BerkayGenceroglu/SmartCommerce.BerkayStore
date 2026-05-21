namespace OrderApi.Dtos
{
    public class AddToCartDto
    {
        public Guid ProductId { get; set; }
        public string ProductName { get; set; } = null!;
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public string? VariantInfo { get; set; } // ← ekle

    }
}
