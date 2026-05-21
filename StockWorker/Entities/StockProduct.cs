using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockWorker.Entities
{
    public class StockProduct
    {
        public Guid Id { get; set; }
        public int Stock { get; set; }
    }
}
