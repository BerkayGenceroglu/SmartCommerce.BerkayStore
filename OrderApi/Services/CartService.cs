using Newtonsoft.Json;
using OrderApi.Dtos;
using OrderApi.Entities;
using StackExchange.Redis;

namespace OrderApi.Services;

public class CartService : ICartService
{
    private readonly IConnectionMultiplexer _redis;
    private const int CartTtlMinutes = 30;

    public CartService(IConnectionMultiplexer redis)
    {
        _redis = redis;
    }

    private string GetCartKey(Guid userId) => $"cart:{userId}";

    public async Task<CartResponseDto> GetCartAsync(Guid userId)
    {
        var db = _redis.GetDatabase();
        var cached = await db.StringGetAsync(GetCartKey(userId));

        if (!cached.HasValue)
            return new CartResponseDto { UserId = userId };

        var cart = JsonConvert.DeserializeObject<Cart>(cached!)!;

        return new CartResponseDto
        {
            UserId = cart.UserId,
            ExpiresAt = cart.ExpiresAt,
            Items = cart.Items.Select(x => new CartItemDto
            {
                ProductId = x.ProductId,
                ProductName = x.ProductName,
                UnitPrice = x.UnitPrice,
                Quantity = x.Quantity,
                VariantInfo = x.VariantInfo  // ← ekle
            }).ToList()
        };
    }

    public async Task<CartResponseDto> AddToCartAsync(Guid userId, AddToCartDto dto)
    {
        var db = _redis.GetDatabase();
        var cached = await db.StringGetAsync(GetCartKey(userId));

        Cart cart;

        if (cached.HasValue)
        {
            cart = JsonConvert.DeserializeObject<Cart>(cached!)!;
        }
        else
        {
            cart = new Cart
            {
                UserId = userId,
                Items = new List<CartItem>()
            };
        }

        // Ürün zaten sepette var mı?
        var existingItem = cart.Items.FirstOrDefault(x => x.ProductId == dto.ProductId);
        if (existingItem != null)
        {
            existingItem.Quantity += dto.Quantity;
        }
        else
        {
            cart.Items.Add(new CartItem
            {
                ProductId = dto.ProductId,
                ProductName = dto.ProductName,
                UnitPrice = dto.UnitPrice,
                Quantity = dto.Quantity,
                VariantInfo = dto.VariantInfo  // ← ekle
            });
        }

        cart.ExpiresAt = DateTime.UtcNow.AddMinutes(CartTtlMinutes);

        await db.StringSetAsync(
            GetCartKey(userId),
            JsonConvert.SerializeObject(cart),
            TimeSpan.FromMinutes(CartTtlMinutes));

        return await GetCartAsync(userId);
    }

    public async Task<CartResponseDto> RemoveFromCartAsync(Guid userId, Guid productId)
    {
        var db = _redis.GetDatabase();
        var cached = await db.StringGetAsync(GetCartKey(userId));

        if (!cached.HasValue)
            throw new Exception("Sepet bulunamadı!");

        var cart = JsonConvert.DeserializeObject<Cart>(cached!)!;
        var item = cart.Items.FirstOrDefault(x => x.ProductId == productId);

        if (item == null)
            throw new Exception("Ürün sepette bulunamadı!");

        cart.Items.Remove(item);
        cart.ExpiresAt = DateTime.UtcNow.AddMinutes(CartTtlMinutes);

        await db.StringSetAsync(
            GetCartKey(userId),
            JsonConvert.SerializeObject(cart),
            TimeSpan.FromMinutes(CartTtlMinutes));

        return await GetCartAsync(userId);
    }

    public async Task ClearCartAsync(Guid userId)
    {
        var db = _redis.GetDatabase();
        await db.KeyDeleteAsync(GetCartKey(userId));
    }
}