using Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Entities
{
    public class OrderCreated
    {
        public Guid OrderId { get; set; }
        public Guid UserId { get; set; }
        public List<OrderCreatedItem> Items { get; set; } = new();
        public OrderStatus Status { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
