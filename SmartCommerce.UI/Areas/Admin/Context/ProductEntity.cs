namespace SmartCommerce.UI.Areas.Admin.Context
{
    public class ProductEntity
    {
      
            public Guid Id { get; set; } = Guid.NewGuid();
            public string Brand { get; set; } = null!;
            public string? ImageUrl { get; set; }
            public string Name { get; set; } = null!;
            public string Description { get; set; } = null!;
            public decimal Price { get; set; }
            public int Stock { get; set; }

    }
}
