using OrderApi.Dtos;

namespace OrderApi.Services
{
    public interface ICartService
    {
        Task<CartResponseDto> GetCartAsync(Guid userId);
        Task<CartResponseDto> AddToCartAsync(Guid userId, AddToCartDto dto);
        Task<CartResponseDto> RemoveFromCartAsync(Guid userId, Guid productId);
        Task ClearCartAsync(Guid userId);
    }
}
