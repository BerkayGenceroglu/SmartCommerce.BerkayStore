using Microsoft.EntityFrameworkCore;
using ProductApi.Context;
using ProductApi.Dtos;
using ProductApi.Entities;

namespace ProductApi.Services;

public class ReviewService : IReviewService
{
    private readonly ProductContext _context;

    public ReviewService(ProductContext context)
    {
        _context = context;
    }

    public async Task<List<ReviewResponseDto>> GetByProductIdAsync(Guid productId)
    {
        return await _context.Reviews
            .Where(x => x.ProductId == productId)
            .OrderByDescending(x => x.CreatedAt)
            .Select(x => new ReviewResponseDto
            {
                Id = x.Id,
                ProductId = x.ProductId,
                UserId = x.UserId,
                UserFullName = x.UserFullName,
                Comment = x.Comment,
                Rating = x.Rating,
                CreatedAt = x.CreatedAt
            })
            .ToListAsync();
    }

    public async Task<ReviewResponseDto> CreateAsync(Guid productId, Guid userId, string userFullName, CreateReviewDto dto)
    {
        // Aynı kullanıcı aynı ürüne iki kez yorum yapmasın
        var existing = await _context.Reviews
            .FirstOrDefaultAsync(x => x.ProductId == productId && x.UserId == userId);

        if (existing != null)
            throw new Exception("Bu ürüne zaten yorum yaptınız!");

        if (dto.Rating < 1 || dto.Rating > 5)
            throw new Exception("Puan 1 ile 5 arasında olmalıdır!");

        var review = new Review
        {
            ProductId = productId,
            UserId = userId,
            UserFullName = userFullName,
            Comment = dto.Comment,
            Rating = dto.Rating
        };

        await _context.Reviews.AddAsync(review);
        await _context.SaveChangesAsync();

        return new ReviewResponseDto
        {
            Id = review.Id,
            ProductId = review.ProductId,
            UserId = review.UserId,
            UserFullName = review.UserFullName,
            Comment = review.Comment,
            Rating = review.Rating,
            CreatedAt = review.CreatedAt
        };
    }

    public async Task DeleteAsync(Guid id, Guid userId)
    {
        var review = await _context.Reviews
            .FirstOrDefaultAsync(x => x.Id == id);

        if (review == null)
            throw new Exception("Yorum bulunamadı!");

        if (review.UserId != userId)
            throw new Exception("Bu yorumu silemezsiniz!");

        _context.Reviews.Remove(review);
        await _context.SaveChangesAsync();
    }
}