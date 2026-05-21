using Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Entities
{
    public class Cargo
    {
        public Guid CargoId { get; set; }
        public Guid OrderId { get; set; }
        public Guid UserId { get; set; }
        public string TrackingNumber { get; set; }
        public CargoStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? DeliveredAt { get; set; }
    }
}
