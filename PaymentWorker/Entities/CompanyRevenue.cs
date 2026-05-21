using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentWorker.Entities
{
    public class CompanyRevenue
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public decimal TotalRevenue { get; set; }
        public int TotalOrders { get; set; }
        public DateTime LastUpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
