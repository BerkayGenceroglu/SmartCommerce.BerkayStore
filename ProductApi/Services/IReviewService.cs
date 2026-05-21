using ProductApi.Dtos;

namespace ProductApi.Services
{
    public interface IReviewService
    {
        Task<List<ReviewResponseDto>> GetByProductIdAsync(Guid productId);
        Task<ReviewResponseDto> CreateAsync(Guid productId, Guid userId, string userFullName, CreateReviewDto dto);
        Task DeleteAsync(Guid id, Guid userId);
    }
}
