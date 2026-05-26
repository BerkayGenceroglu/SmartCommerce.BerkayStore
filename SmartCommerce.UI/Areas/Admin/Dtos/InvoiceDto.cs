namespace SmartCommerce.UI.Areas.Admin.Dtos
{
    public class InvoiceDto
    {
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        public Guid UserId { get; set; }
        public decimal TotalAmount { get; set; }
        public string InvoiceNumber { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
    }
}
