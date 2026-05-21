namespace ProductApi.Dtos
{
    public class ProductResponseDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Brand { get; set; } = null!;
        public string? ImageUrl { get; set; }
        public string Description { get; set; } = null!;
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public bool IsActive { get; set; }
        public string CategoryName { get; set; } = null!;
        public DateTime CreatedAt { get; set; }

        public List<ProductVariantDto> Variants { get; set; } = new();

    }
}
