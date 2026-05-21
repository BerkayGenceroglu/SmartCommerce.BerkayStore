using ProductApi.Dtos;

namespace ProductApi.Services
{
    public interface IFavoriteService
    {
        Task<List<FavoriteResponseDto>> GetUserFavoritesAsync(Guid userId);
        Task<FavoriteResponseDto> AddFavoriteAsync(Guid userId, Guid productId);
        Task RemoveFavoriteAsync(Guid userId, Guid productId);
        Task<bool> IsFavoriteAsync(Guid userId, Guid productId);
    }
}
