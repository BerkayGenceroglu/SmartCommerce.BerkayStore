namespace ProductApi.Entities
{
    public class Review
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid ProductId { get; set; }
        public Product Product { get; set; } = null!;
        public Guid UserId { get; set; }
        public string UserFullName { get; set; } = null!;
        public string Comment { get; set; } = null!;
        public int Rating { get; set; } // 1-5
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
