using MassTransit;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using OrderApi.Context;
using OrderApi.Dtos;
using OrderApi.Entities;
using Shared.Entities;
using Shared.Enums;
using StackExchange.Redis;

namespace OrderApi.Services;

public class OrderService : IOrderService
{
    private readonly OrderDbContext _context;
    private readonly IConnectionMultiplexer _redis;
    private readonly IPublishEndpoint _publishEndpoint;

    public OrderService(OrderDbContext context, IConnectionMultiplexer redis, IPublishEndpoint publishEndpoint)
    {
        _context = context;
        _redis = redis;
        _publishEndpoint = publishEndpoint;
    }

    public async Task<OrderResponseDto> CreateOrderAsync(Guid userId, CreateOrderDto dto)
    {
        var db = _redis.GetDatabase();
        var cartKey = $"cart:{userId}";
        var cached = await db.StringGetAsync(cartKey);

        if (!cached.HasValue)
            throw new Exception("Sepet boş veya süresi dolmuş!");

        var cart = JsonConvert.DeserializeObject<Cart>(cached!)!;

        if (!cart.Items.Any())
            throw new Exception("Sepette ürün yok!");

        var user = await _context.Users
            .FirstOrDefaultAsync(x => x.Id == userId);

        if (user == null)
            throw new Exception("Kullanıcı bulunamadı!");

        // Toplam tutar
        var totalAmount = cart.Items.Sum(x => x.UnitPrice * x.Quantity);

        // Kupon varsa uygula
        if (!string.IsNullOrEmpty(dto.CouponCode))
        {
            var coupon = await _context.Coupons
                .FirstOrDefaultAsync(x => x.Code.ToUpper() == dto.CouponCode.ToUpper());

            if (coupon != null && coupon.IsActive && coupon.ExpiresAt > DateTime.UtcNow
                && coupon.UsageCount < coupon.UsageLimit && totalAmount >= coupon.MinimumAmount)
            {
                decimal discount = coupon.DiscountType == DiscountType.Percentage
                    ? totalAmount * coupon.DiscountValue / 100
                    : coupon.DiscountValue;

                if (discount > totalAmount) discount = totalAmount;
                totalAmount -= discount;

                // Kullanım sayısını artır
                coupon.UsageCount++;
                _context.Coupons.Update(coupon);
            }
        }

        var order = new Entities.Order
        {
            UserId = userId,
            Address = dto.Address,
            Status = OrderStatus.Pending,
            Items = cart.Items.Select(x => new OrderItem
            {
                ProductId = x.ProductId,
                ProductName = x.ProductName,
                UnitPrice = x.UnitPrice,
                Quantity = x.Quantity
            }).ToList(),
            TotalAmount = totalAmount  // ← indirimli tutar
        };

        await _context.Orders.AddAsync(order);
        await _context.SaveChangesAsync();

        // RabbitMQ'ya event yayınla
        await _publishEndpoint.Publish(new OrderCreated
        {
            OrderId = order.Id,
            UserId = order.UserId,
            TotalAmount = order.TotalAmount,
            CreatedAt = order.CreatedAt,
            Items = order.Items.Select(x => new OrderCreatedItem
            {
                ProductId = x.ProductId,
                ProductName = x.ProductName,
                UnitPrice = x.UnitPrice,
                Quantity = x.Quantity
            }).ToList()
        });

        // Sepeti temizle
        await db.KeyDeleteAsync(cartKey);

        return new OrderResponseDto
        {
            Id = order.Id,
            UserId = order.UserId,
            TotalAmount = order.TotalAmount,
            Status = order.Status,
            Address = order.Address,
            CreatedAt = order.CreatedAt,
            Items = order.Items.Select(x => new OrderItemDto
            {
                ProductId = x.ProductId,
                ProductName = x.ProductName,
                UnitPrice = x.UnitPrice,
                Quantity = x.Quantity
            }).ToList()
        };
    }

    public async Task<List<OrderResponseDto>> GetUserOrdersAsync(Guid userId)
    {
        return await _context.Orders
            .Include(x => x.Items)
            .Where(x => x.UserId == userId)
            .OrderByDescending(x => x.CreatedAt)  // ← ekle
            .Select(x => new OrderResponseDto
            {
                Id = x.Id,
                UserId = x.UserId,
                TotalAmount = x.TotalAmount,
                Status = x.Status,
                Address = x.Address,
                CreatedAt = x.CreatedAt,
                Items = x.Items.Select(i => new OrderItemDto
                {
                    ProductId = i.ProductId,
                    ProductName = i.ProductName,
                    UnitPrice = i.UnitPrice,
                    Quantity = i.Quantity,
                    
                }).ToList()
            })
            .ToListAsync();
    }

    public async Task<OrderResponseDto> GetOrderByIdAsync(Guid id)
    {
        var order = await _context.Orders
            .Include(x => x.Items)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (order == null)
            throw new Exception("Sipariş bulunamadı!");

        return new OrderResponseDto
        {
            Id = order.Id,
            UserId = order.UserId,
            TotalAmount = order.TotalAmount,
            Status = order.Status,
            Address = order.Address,
            CreatedAt = order.CreatedAt,
            Items = order.Items.Select(x => new OrderItemDto
            {
                ProductId = x.ProductId,
                ProductName = x.ProductName,
                UnitPrice = x.UnitPrice,
                Quantity = x.Quantity
            }).ToList()
        };
    }
}