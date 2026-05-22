using Shared.Enums;

namespace SmartCommerce.UI.Areas.Admin.Context
{
    public class CargoEntity
    {
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        public string TrackingNumber { get; set; } = null!;
        public CargoStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
