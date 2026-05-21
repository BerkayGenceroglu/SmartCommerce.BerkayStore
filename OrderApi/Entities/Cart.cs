namespace OrderApi.Entities
{
    public class Cart
    {
        public Guid UserId { get; set; }
        public List<CartItem> Items { get; set; } = new();
        public DateTime ExpiresAt { get; set; }
    }
}
