namespace ProductApi.Dtos
{
    public class ProductVariantDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Value { get; set; } = null!;
        public int Stock { get; set; }
        public decimal? PriceModifier { get; set; }
    }
    public class CreateProductVariantDto
    {
        public string Name { get; set; } = null!;
        public string Value { get; set; } = null!;
        public int Stock { get; set; }
        public decimal? PriceModifier { get; set; }
    }
}
