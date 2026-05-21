using Microsoft.EntityFrameworkCore;
using ProductApi.Context;
using ProductApi.Dtos;
using ProductApi.Entities;

namespace ProductApi.Services;

public class FavoriteService : IFavoriteService
{
    private readonly ProductContext _context;

    public FavoriteService(ProductContext context)
    {
        _context = context;
    }

    public async Task<List<FavoriteResponseDto>> GetUserFavoritesAsync(Guid userId)
    {
        return await _context.Favorites
            .Include(x => x.Product)
            .ThenInclude(x => x.Category)
            .Where(x => x.UserId == userId)
            .OrderByDescending(x => x.CreatedAt)
            .Select(x => new FavoriteResponseDto
            {
                Id = x.Id,
                UserId = x.UserId,
                ProductId = x.ProductId,
                ProductName = x.Product.Name,
                Brand = x.Product.Brand,
                ImageUrl = x.Product.ImageUrl,
                Price = x.Product.Price,
                Stock = x.Product.Stock,
                CategoryName = x.Product.Category.Name,
                CreatedAt = x.CreatedAt
            })
            .ToListAsync();
    }

    public async Task<FavoriteResponseDto> AddFavoriteAsync(Guid userId, Guid productId)
    {
        var existing = await _context.Favorites
            .FirstOrDefaultAsync(x => x.UserId == userId && x.ProductId == productId);

        if (existing != null)
            throw new Exception("Bu ürün zaten favorilerinizde!");

        var product = await _context.Products
            .Include(x => x.Category)
            .FirstOrDefaultAsync(x => x.Id == productId);

        if (product == null)
            throw new Exception("Ürün bulunamadı!");

        var favorite = new Favorite
        {
            UserId = userId,
            ProductId = productId
        };

        await _context.Favorites.AddAsync(favorite);
        await _context.SaveChangesAsync();

        return new FavoriteResponseDto
        {
            Id = favorite.Id,
            UserId = favorite.UserId,
            ProductId = favorite.ProductId,
            ProductName = product.Name,
            Brand = product.Brand,
            ImageUrl = product.ImageUrl,
            Price = product.Price,
            Stock = product.Stock,
            CategoryName = product.Category.Name,
            CreatedAt = favorite.CreatedAt
        };
    }

    public async Task RemoveFavoriteAsync(Guid userId, Guid productId)
    {
        var favorite = await _context.Favorites
            .FirstOrDefaultAsync(x => x.UserId == userId && x.ProductId == productId);

        if (favorite == null)
            throw new Exception("Favori bulunamadı!");

        _context.Favorites.Remove(favorite);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> IsFavoriteAsync(Guid userId, Guid productId)
    {
        return await _context.Favorites
            .AnyAsync(x => x.UserId == userId && x.ProductId == productId);
    }
}