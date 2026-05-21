using MassTransit;
using Microsoft.EntityFrameworkCore;
using NotificationWorker.Context;
using NotificationWorker.Entities;
using NotificationWorker.Services;
using Shared.Entities;

namespace NotificationWorker.Consumers;

public class OrderCreatedConsumer : IConsumer<OrderCreated>
{
    private readonly NotificationDbContext _context;
    private readonly IMailService _mailService;
    private readonly ILogger<OrderCreatedConsumer> _logger;

    public OrderCreatedConsumer(
        NotificationDbContext context,
        IMailService mailService,
        ILogger<OrderCreatedConsumer> logger)
    {
        _context = context;
        _mailService = mailService;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<OrderCreated> context)
    {
        var message = context.Message;

        _logger.LogInformation("OrderCreated eventi alındı. OrderId: {OrderId}", message.OrderId);

        // Kullanıcıyı çek
        var user = await _context.Users
            .FirstOrDefaultAsync(x => x.Id == message.UserId);

        if (user == null)
        {
            _logger.LogWarning("Kullanıcı bulunamadı. UserId: {UserId}", message.UserId);
            return;
        }

        // DB'ye bildirim kaydet
        var notification = new Notification
        {
            UserId = message.UserId,
            OrderId = message.OrderId,
            Message = $"Siparişiniz alındı! Sipariş No: {message.OrderId}, Toplam: {message.TotalAmount}₺"
        };

        await _context.Notifications.AddAsync(notification);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Bildirim oluşturuldu. UserId: {UserId}", message.UserId);

        // Mail gönder
        await _mailService.SendOrderConfirmationAsync(
            toEmail: user.Email,
            fullName: user.FullName,
            orderId: message.OrderId,
            totalAmount: message.TotalAmount);
    }
}