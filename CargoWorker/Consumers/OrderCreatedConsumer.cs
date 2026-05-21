using CargoWorker.Context;
using CargoWorker.Entities;
using MassTransit;
using Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CargoWorker.Consumers
{
    public class OrderCreatedConsumer : IConsumer<OrderCreated>
    {
        private readonly CargoDbContext _context;
        private readonly ILogger<OrderCreatedConsumer> _logger;

        public OrderCreatedConsumer(CargoDbContext context, ILogger<OrderCreatedConsumer> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<OrderCreated> context)
        {
            var message = context.Message;

            _logger.LogInformation("Kargo kaydı oluşturuluyor. OrderId: {OrderId}", message.OrderId);

            var cargo = new CargoRecord
            {
                OrderId = message.OrderId,
                UserId = message.UserId,
                TrackingNumber = $"CARGO-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString()[..8].ToUpper()}"
            };

            await _context.Cargos.AddAsync(cargo);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Kargo oluşturuldu. TrackingNumber: {TrackingNumber}", cargo.TrackingNumber);
        }
    }
}
