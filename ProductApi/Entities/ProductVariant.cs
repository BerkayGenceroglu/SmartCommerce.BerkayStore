namespace ProductApi.Entities
{
    public class ProductVariant
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid ProductId { get; set; }
        public Product Product { get; set; } = null!;
        public string Name { get; set; } = null!; // "Beden", "Numara", "Renk"
        public string Value { get; set; } = null!; // "S", "M", "L", "42", "Siyah"
        public int Stock { get; set; }
        public decimal? PriceModifier { get; set; } // Fiyat farkı (opsiyonel)
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
