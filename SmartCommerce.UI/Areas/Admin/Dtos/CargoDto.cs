using Shared.Enums;

namespace SmartCommerce.UI.Areas.Admin.Dtos
{
    public class CargoDto
    {
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        public string TrackingNumber { get; set; } = null!;
        public string Status { get; set; }  // CargoStatus → string
        public DateTime CreatedAt { get; set; }
    }
}
