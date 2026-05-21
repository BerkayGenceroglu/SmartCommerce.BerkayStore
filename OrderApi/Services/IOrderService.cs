using OrderApi.Dtos;

namespace OrderApi.Services
{
    public interface IOrderService
    {
        Task<OrderResponseDto> CreateOrderAsync(Guid userId, CreateOrderDto dto);
        Task<List<OrderResponseDto>> GetUserOrdersAsync(Guid userId);
        Task<OrderResponseDto> GetOrderByIdAsync(Guid id);
    }
}
