using Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CargoWorker.Entities
{
    public class CargoRecord
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid OrderId { get; set; }
        public Guid UserId { get; set; }
        public string TrackingNumber { get; set; } = null!;
        public CargoStatus Status { get; set; } = CargoStatus.Preparing;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? DeliveredAt { get; set; }
    }
}
