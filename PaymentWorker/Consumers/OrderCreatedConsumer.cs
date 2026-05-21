using MassTransit;
using Microsoft.EntityFrameworkCore;
using PaymentWorker.Context;
using PaymentWorker.Entities;
using Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentWorker.Consumers
{
    public class OrderCreatedConsumer : IConsumer<OrderCreated>
    {
        private readonly PaymentDbContext _context;
        private readonly ILogger<OrderCreatedConsumer> _logger;

        public OrderCreatedConsumer(PaymentDbContext context, ILogger<OrderCreatedConsumer> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<OrderCreated> context)
        {
            var message = context.Message;

            _logger.LogInformation("Ödeme işlemi başladı. OrderId: {OrderId}", message.OrderId);

            // %80 başarılı, %20 başarısız
            var isSuccessful = new Random().Next(1, 101) <= 80;

            var payment = new Payment
            {
                OrderId = message.OrderId,
                UserId = message.UserId,
                Amount = message.TotalAmount,
                IsSuccessful = isSuccessful,
                FailReason = isSuccessful ? null : "Yetersiz bakiye"
            };

            await _context.Payments.AddAsync(payment);

            if (isSuccessful)
            {
                // Şirket gelirini güncelle
                var revenue = await _context.CompanyRevenues.FirstOrDefaultAsync();

                if (revenue == null)
                {
                    revenue = new CompanyRevenue
                    {
                        TotalRevenue = message.TotalAmount,
                        TotalOrders = 1
                    };
                    await _context.CompanyRevenues.AddAsync(revenue);
                }
                else
                {
                    revenue.TotalRevenue += message.TotalAmount;
                    revenue.TotalOrders += 1;
                    revenue.LastUpdatedAt = DateTime.UtcNow;
                }

                _logger.LogInformation("Ödeme başarılı. OrderId: {OrderId}, Amount: {Amount}", message.OrderId, message.TotalAmount);
            }
            else
            {
                _logger.LogWarning("Ödeme başarısız. OrderId: {OrderId}, Reason: {Reason}", message.OrderId, payment.FailReason);
            }

            await _context.SaveChangesAsync();
        }
    }
}
