using MassTransit;
using Microsoft.EntityFrameworkCore;
using Shared.Entities;
using StockWorker.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockWorker.Consumers
{
    public class OrderCreatedConsumer : IConsumer<OrderCreated>
    {
        private readonly StockDbContext _context;
        private readonly ILogger<OrderCreatedConsumer> _logger;

        public OrderCreatedConsumer(StockDbContext context, ILogger<OrderCreatedConsumer> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<OrderCreated> context)
        {
            var message = context.Message;

            _logger.LogInformation("Stok güncelleniyor. OrderId: {OrderId}", message.OrderId);

            foreach (var item in message.Items)
            {
                var product = await _context.Products
                    .FirstOrDefaultAsync(x => x.Id == item.ProductId);

                if (product == null)
                {
                    _logger.LogWarning("Ürün bulunamadı. ProductId: {ProductId}", item.ProductId);
                    continue;
                }

                product.Stock -= item.Quantity;
                _logger.LogInformation("Stok düşüldü. ProductId: {ProductId}, Yeni Stok: {Stock}", item.ProductId, product.Stock);
            }

            await _context.SaveChangesAsync();
        }
    }
}
