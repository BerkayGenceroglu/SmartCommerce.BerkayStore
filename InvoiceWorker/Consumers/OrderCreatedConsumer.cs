using InvoiceWorker.Context;
using InvoiceWorker.Entities;
using MassTransit;
using Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoiceWorker.Consumers
{
    public class OrderCreatedConsumer : IConsumer<OrderCreated>
    {
        private readonly InvoiceDbContext _context;
        private readonly ILogger<OrderCreatedConsumer> _logger;

        public OrderCreatedConsumer(InvoiceDbContext context, ILogger<OrderCreatedConsumer> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task Consume(ConsumeContext<OrderCreated> context)
        {
            var message = context.Message;

            _logger.LogInformation("Fatura oluşturuluyor. OrderId: {OrderId}", message.OrderId);

            var invoice = new Invoice
            {
                OrderId = message.OrderId,
                UserId = message.UserId,
                TotalAmount = message.TotalAmount,
                InvoiceNumber = $"INV-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString()[..8].ToUpper()}"
            };

            await _context.Invoices.AddAsync(invoice);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Fatura oluşturuldu. InvoiceNumber: {InvoiceNumber}", invoice.InvoiceNumber);
        }
    }
}
