using Microsoft.EntityFrameworkCore;
using SmartCommerce.UI.Areas.Admin.Abstract;
using SmartCommerce.UI.Areas.Admin.Context;
using SmartCommerce.UI.Areas.Admin.Dtos;

namespace SmartCommerce.UI.Areas.Admin.Services
{
    public class ReviewService : IReviewService
    {
        private readonly AdminDbContext _context;

        public ReviewService(AdminDbContext context)
        {
            _context = context;
        }
        public async Task<List<ReviewDto>> GetReviewListsAsync()
        {
            var reviews = await _context.Reviews.Select(r => new ReviewDto
            {
                Id = r.Id,
                ProductId = r.ProductId,
                UserId = r.UserId,
                Comment = r.Comment,
                Rating = r.Rating,
                CreatedAt = r.CreatedAt
            }).ToListAsync();
            return reviews;
        }
    }
}
