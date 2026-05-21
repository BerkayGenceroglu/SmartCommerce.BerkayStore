using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductApi.Context;
using ProductApi.Dtos;
using StackExchange.Redis;

namespace ProductApi.Controllers;

[ApiController]
[Route("api/product/{productId}/variants")]
public class ProductVariantController : ControllerBase
{
    private readonly ProductContext _context;
    private readonly IConnectionMultiplexer _redis;
    private const string CacheKey = "products:all";

    public ProductVariantController(ProductContext context, IConnectionMultiplexer redis)
    {
        _context = context;
        _redis = redis;
    }

    [HttpGet]
    public async Task<IActionResult> GetVariants(Guid productId)
    {
        var variants = await _context.ProductVariants
            .Where(x => x.ProductId == productId)
            .Select(x => new ProductVariantDto
            {
                Id = x.Id,
                Name = x.Name,
                Value = x.Value,
                Stock = x.Stock,
                PriceModifier = x.PriceModifier
            })
            .ToListAsync();

        return Ok(variants);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> AddVariant(Guid productId, CreateProductVariantDto dto)
    {
        var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == productId);
        if (product == null) return NotFound("Ürün bulunamadı!");

        var variant = new Entities.ProductVariant
        {
            ProductId = productId,
            Name = dto.Name,
            Value = dto.Value,
            Stock = dto.Stock,
            PriceModifier = dto.PriceModifier
        };

        await _context.ProductVariants.AddAsync(variant);
        await _context.SaveChangesAsync();

        // Cache temizle
        var db = _redis.GetDatabase();
        await db.KeyDeleteAsync(CacheKey);
        await db.KeyDeleteAsync($"products:{productId}");

        return Ok(new ProductVariantDto
        {
            Id = variant.Id,
            Name = variant.Name,
            Value = variant.Value,
            Stock = variant.Stock,
            PriceModifier = variant.PriceModifier
        });
    }

    [HttpDelete("{variantId}")]
    [Authorize]
    public async Task<IActionResult> DeleteVariant(Guid productId, Guid variantId)
    {
        var variant = await _context.ProductVariants
            .FirstOrDefaultAsync(x => x.Id == variantId && x.ProductId == productId);

        if (variant == null) return NotFound("Varyant bulunamadı!");

        _context.ProductVariants.Remove(variant);
        await _context.SaveChangesAsync();

        var db = _redis.GetDatabase();
        await db.KeyDeleteAsync(CacheKey);
        await db.KeyDeleteAsync($"products:{productId}");

        return Ok("Varyant silindi.");
    }
}