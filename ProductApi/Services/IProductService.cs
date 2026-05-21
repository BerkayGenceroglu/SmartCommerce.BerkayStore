using ProductApi.Dtos;

namespace ProductApi.Services
{
    public interface IProductService
    {
        Task ReIndexAsync();
        Task<List<ProductResponseDto>> GetAllAsync();
        Task<ProductResponseDto> GetByIdAsync(Guid id);
        Task<List<ProductResponseDto>> SearchAsync(string query);
        Task<ProductResponseDto> CreateAsync(CreateProductDto dto);
        Task<ProductResponseDto> UpdateAsync(Guid id, UpdateProductDto dto);
        Task DeleteAsync(Guid id);
    }
}
