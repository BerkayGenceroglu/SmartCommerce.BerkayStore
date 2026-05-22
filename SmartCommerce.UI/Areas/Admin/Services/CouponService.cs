using SmartCommerce.UI.Areas.Admin.Abstract;
using SmartCommerce.UI.Areas.Admin.Dtos;
using System.Net.Http.Headers;

namespace SmartCommerce.UI.Areas.Admin.Services
{
    public class CouponService : ICouponService
    {
        private readonly HttpClient _http;

        public CouponService(IHttpClientFactory httpClientFactory)
        {
            _http = httpClientFactory.CreateClient();
        }

        public async Task<List<CouponDto>> GetAllAsync()
        {
            try
            {
                var result = await _http.GetFromJsonAsync<List<CouponDto>>("https://localhost:7124/api/coupons");
                return result ?? new();
            }
            catch { return new(); }
        }

        public async Task<bool> CreateAsync(CreateCouponDto dto, string token)
        {
            try
            {
                _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var res = await _http.PostAsJsonAsync("https://localhost:7124/api/coupons", dto);
                return res.IsSuccessStatusCode;
            }
            catch { return false; }
        }

        public async Task<bool> DeleteAsync(Guid id, string token)
        {
            try
            {
                _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var res = await _http.DeleteAsync($"https://localhost:7124/api/coupons/{id}");
                return res.IsSuccessStatusCode;
            }
            catch { return false; }
        }
    }
}
