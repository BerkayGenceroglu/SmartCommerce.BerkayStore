using SmartCommerce.UI.Areas.Admin.Abstract;
using SmartCommerce.UI.Areas.Admin.Dtos;

namespace SmartCommerce.UI.Areas.Admin.Services
{
    public class AdminProductService : IAdminProductService
    {
        private readonly HttpClient _http;

        public AdminProductService(IHttpClientFactory httpClientFactory)
        {
            _http = httpClientFactory.CreateClient();
        }

        public async Task<List<ProductDto>> GetAllAsync()
        {
            try
            {
                var result = await _http.GetFromJsonAsync<List<ProductDto>>("https://localhost:7136/api/product");
                return result ?? new();
            }
            catch { return new(); }
        }

        public async Task<List<CategoryDto>> GetCategoriesAsync()
        {
            try
            {
                var result = await _http.GetFromJsonAsync<List<CategoryDto>>("https://localhost:7136/api/category");
                return result ?? new();
            }
            catch { return new(); }
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            try
            {
                var res = await _http.DeleteAsync($"https://localhost:7136/api/product/{id}");
                return res.IsSuccessStatusCode;
            }
            catch { return false; }
        }
    }
}
