using SmartCommerce.UI.Areas.Admin.Dtos;

namespace SmartCommerce.UI.Areas.Admin.Abstract
{
    public interface IAdminProductService
    {
        Task<List<ProductDto>> GetAllAsync();
        Task<List<CategoryDto>> GetCategoriesAsync();
        Task<bool> DeleteAsync(Guid id);
    }
}
