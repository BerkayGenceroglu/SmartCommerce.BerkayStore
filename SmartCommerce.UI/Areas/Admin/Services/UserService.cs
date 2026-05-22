using SmartCommerce.UI.Areas.Admin.Abstract;
using SmartCommerce.UI.Areas.Admin.Dtos;

namespace SmartCommerce.UI.Areas.Admin.Services
{
    public class UserService :IUserService
    {
        private readonly HttpClient _http;

        public UserService(IHttpClientFactory httpClientFactory)
        {
            _http = httpClientFactory.CreateClient();
        }

        public async Task<List<UserDto>> GetAllAsync()
        {
            try
            {
                var result = await _http.GetFromJsonAsync<List<UserDto>>("https://localhost:7038/api/user/all");
                return result ?? new();
            }
            catch { return new(); }
        }
    }
}
