namespace SmartCommerce.UI.Areas.Admin.Context
{
    public class ReviewEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid ProductId { get; set; }
        public ProductEntity Product { get; set; } = null!;
        public Guid UserId { get; set; }
        public string UserFullName { get; set; } = null!;
        public string Comment { get; set; } = null!;
        public int Rating { get; set; } // 1-5
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}

