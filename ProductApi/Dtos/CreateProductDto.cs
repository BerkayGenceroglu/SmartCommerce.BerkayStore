namespace ProductApi.Dtos
{
    public class CreateProductDto
    {
        public string Name { get; set; } = null!;
        public string Brand { get; set; } = null!;
        public string? ImageUrl { get; set; }
        public string Description { get; set; } = null!;
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public Guid CategoryId { get; set; }
    }
}
