using Microsoft.EntityFrameworkCore;
using Nest;
using ProductApi.Context;
using ProductApi.Dtos;
using ProductApi.Entities;
using StackExchange.Redis;
using Newtonsoft.Json;

namespace ProductApi.Services;

public class ProductService : IProductService
{
    private readonly ProductContext _context;
    private readonly IConnectionMultiplexer _redis;
    private readonly IElasticClient _elasticClient;
    private const string CacheKey = "products:all";

    public ProductService(ProductContext context, IConnectionMultiplexer redis, IElasticClient elasticClient)
    {
        _context = context;
        _redis = redis;
        _elasticClient = elasticClient;
    }

    public async Task ReIndexAsync()
    {
        var products = await _context.Products
            .Include(x => x.Category)
            .Where(x => x.IsActive)
            .ToListAsync();

        foreach (var product in products)
        {
            await _elasticClient.IndexAsync(new
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Brand = product.Brand,
                CategoryId = product.CategoryId,
                Price = product.Price,
                Stock = product.Stock,
                CreatedAt = product.CreatedAt
            }, i => i.Index("products").Id(product.Id.ToString()));
        }
    }

    public async Task<List<ProductResponseDto>> GetAllAsync()
    {
        var db = _redis.GetDatabase();
        var cached = await db.StringGetAsync(CacheKey);

        if (cached.HasValue)
            return JsonConvert.DeserializeObject<List<ProductResponseDto>>(cached!)!;

        var products = await _context.Products
            .Include(x => x.Category)
            .Include(x => x.Variants)
            .Where(x => x.IsActive)
            .Select(x => new ProductResponseDto
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                Brand = x.Brand,
                ImageUrl = x.ImageUrl,
                Price = x.Price,
                Stock = x.Stock,
                IsActive = x.IsActive,
                CategoryName = x.Category.Name,
                CreatedAt = x.CreatedAt,
                Variants = x.Variants.Select(v => new ProductVariantDto
                {
                    Id = v.Id,
                    Name = v.Name,
                    Value = v.Value,
                    Stock = v.Stock,
                    PriceModifier = v.PriceModifier
                }).ToList()
            })
            .ToListAsync();

        await db.StringSetAsync(CacheKey, JsonConvert.SerializeObject(products), TimeSpan.FromMinutes(5));

        return products;
    }

    public async Task<ProductResponseDto> GetByIdAsync(Guid id)
    {
        var db = _redis.GetDatabase();
        var cacheKey = $"products:{id}";
        var cached = await db.StringGetAsync(cacheKey);

        if (cached.HasValue)
            return JsonConvert.DeserializeObject<ProductResponseDto>(cached!)!;

        var product = await _context.Products
            .Include(x => x.Category)
            .Include(x => x.Variants)  // ← ekle
            .FirstOrDefaultAsync(x => x.Id == id);

        if (product == null)
            throw new Exception("Ürün bulunamadı!");

        var dto = new ProductResponseDto
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Brand = product.Brand,
            ImageUrl = product.ImageUrl,
            Price = product.Price,
            Stock = product.Stock,
            IsActive = product.IsActive,
            CategoryName = product.Category.Name,
            CreatedAt = product.CreatedAt,
            Variants = product.Variants.Select(v => new ProductVariantDto
            {
                Id = v.Id,
                Name = v.Name,
                Value = v.Value,
                Stock = v.Stock,
                PriceModifier = v.PriceModifier
            }).ToList()
        };

        await db.StringSetAsync(cacheKey, JsonConvert.SerializeObject(dto), TimeSpan.FromMinutes(5));

        return dto;
    }

    public async Task<List<ProductResponseDto>> SearchAsync(string query)
    {
        var response = await _elasticClient.SearchAsync<Product>(s => s
            .Query(q => q
                .MultiMatch(m => m
                    .Fields(f => f
                        .Field(p => p.Name)
                        .Field(p => p.Description))
                    .Query(query)
                    .Fuzziness(Fuzziness.Auto))));

        var ids = response.Documents.Select(x => x.Id).ToList();

        return await _context.Products
            .Include(x => x.Category)
            .Include(x => x.Variants)
            .Where(x => ids.Contains(x.Id))
            .Select(x => new ProductResponseDto
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                Brand = x.Brand,
                ImageUrl = x.ImageUrl,
                Price = x.Price,
                Stock = x.Stock,
                IsActive = x.IsActive,
                CategoryName = x.Category.Name,
                CreatedAt = x.CreatedAt,
                Variants = x.Variants.Select(v => new ProductVariantDto
                {
                    Id = v.Id,
                    Name = v.Name,
                    Value = v.Value,
                    Stock = v.Stock,
                    PriceModifier = v.PriceModifier
                }).ToList()
            })
            .ToListAsync();
    }

    public async Task<ProductResponseDto> CreateAsync(CreateProductDto dto)
    {
        var category = await _context.Categories
            .FirstOrDefaultAsync(x => x.Id == dto.CategoryId);

        if (category == null)
            throw new Exception("Kategori bulunamadı!");

        var product = new Product
        {
            Name = dto.Name,
            Description = dto.Description,
            Brand = dto.Brand,
            ImageUrl = dto.ImageUrl,
            Price = dto.Price,
            Stock = dto.Stock,
            CategoryId = dto.CategoryId
        };

        await _context.Products.AddAsync(product);
        await _context.SaveChangesAsync();

        // Elasticsearch'e index'le
        await _elasticClient.IndexAsync(new
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Brand = product.Brand,
            ImageUrl = product.ImageUrl,
            Price = product.Price,
            Stock = product.Stock,
            CategoryId = product.CategoryId,
            CreatedAt = product.CreatedAt
        }, i => i.Index("products"));

        // Redis cache temizle
        var db = _redis.GetDatabase();
        await db.KeyDeleteAsync(CacheKey);

        return new ProductResponseDto
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Brand = product.Brand,
            ImageUrl = product.ImageUrl,
            Price = product.Price,
            Stock = product.Stock,
            IsActive = product.IsActive,
            CategoryName = category.Name,
            CreatedAt = product.CreatedAt
        };
    }

    public async Task<ProductResponseDto> UpdateAsync(Guid id, UpdateProductDto dto)
    {
        var product = await _context.Products
            .Include(x => x.Category)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (product == null)
            throw new Exception("Ürün bulunamadı!");

        product.Name = dto.Name;
        product.Description = dto.Description;
        product.Brand = dto.Brand;
        product.ImageUrl = dto.ImageUrl;
        product.Price = dto.Price;
        product.Stock = dto.Stock;
        product.CategoryId = dto.CategoryId;
        product.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        // Elasticsearch güncelle
        await _elasticClient.IndexAsync(new
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Brand = product.Brand,
            ImageUrl = product.ImageUrl,
            Price = product.Price,
            Stock = product.Stock,
            CategoryId = product.CategoryId,
            CreatedAt = product.CreatedAt
        }, i => i.Index("products"));

        // Redis cache temizle
        var db = _redis.GetDatabase();
        await db.KeyDeleteAsync(CacheKey);
        await db.KeyDeleteAsync($"products:{id}");

        return new ProductResponseDto
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Brand = product.Brand,
            ImageUrl = product.ImageUrl,
            Price = product.Price,
            Stock = product.Stock,
            IsActive = product.IsActive,
            CategoryName = product.Category.Name,
            CreatedAt = product.CreatedAt
        };
    }

    public async Task DeleteAsync(Guid id)
    {
        var product = await _context.Products
            .FirstOrDefaultAsync(x => x.Id == id);

        if (product == null)
            throw new Exception("Ürün bulunamadı!");

        _context.Products.Remove(product);
        await _context.SaveChangesAsync();

        // Elasticsearch'ten sil
        await _elasticClient.DeleteAsync<Product>(id, d => d.Index("products"));

        // Redis cache temizle
        var db = _redis.GetDatabase();
        await db.KeyDeleteAsync(CacheKey);
        await db.KeyDeleteAsync($"products:{id}");
    }
}
