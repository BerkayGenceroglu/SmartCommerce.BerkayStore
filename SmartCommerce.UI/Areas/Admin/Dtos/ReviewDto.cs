using Shared.Entities;

namespace SmartCommerce.UI.Areas.Admin.Dtos
{
    public class ReviewDto
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid ProductId { get; set; }
        public Guid UserId { get; set; }
        public string UserFullName { get; set; } = null!;
        public string Comment { get; set; } = null!;
        public int Rating { get; set; } // 1-5
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
    
}
