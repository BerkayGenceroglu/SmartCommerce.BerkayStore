using SmartCommerce.UI.Areas.Admin.Abstract;
using SmartCommerce.UI.Areas.Admin.Dtos;

namespace SmartCommerce.UI.Areas.Admin.Services
{
    public class AdminOrderService : IAdminOrderService
    {
        private readonly HttpClient _http;

        public AdminOrderService(IHttpClientFactory httpClientFactory)
        {
            _http = httpClientFactory.CreateClient();
        }

        public async Task<List<OrderDto>> GetAllAsync()
        {
            try
            {
                var result = await _http.GetFromJsonAsync<List<OrderDto>>("https://localhost:7124/api/order/all");
                return result ?? new();
            }
            catch { return new(); }
        }
    }
}
