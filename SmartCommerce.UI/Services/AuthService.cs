
using Newtonsoft.Json;
using SmartCommerce.UI.Models;

namespace SmartCommerce.UI.Services
{
    public class AuthService : IAuthService
    {

        private readonly HttpClient _httpClient;
        private const string UserApiUrl = "https://localhost:7038";

        public AuthService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<AuthResult> LoginAsync(string email, string password)
        {
            try
            {
                var res = await _httpClient.PostAsJsonAsync($"{UserApiUrl}/api/auth/login", new
                {
                    email,
                    password
                });

                if (!res.IsSuccessStatusCode)
                {
                    var err = await res.Content.ReadAsStringAsync();
                    return new AuthResult { Success = false, Error = err };
                }

                var content = await res.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<AuthResponseModel>(content)!;

                return new AuthResult
                {
                    Success = true,
                    Token = data.Token,
                    FullName = data.FullName,
                    UserId = data.UserId
                };
            }
            catch (Exception ex)
            {
                return new AuthResult { Success = false, Error = ex.Message };
            }
        }

        public async Task<bool> RegisterAsync(string fullName, string email, string password, string? phoneNumber, string? gender, string? city)
        {
            try
            {
                var res = await _httpClient.PostAsJsonAsync($"{UserApiUrl}/api/auth/register", new
                {
                    fullName,
                    email,
                    password,
                    phoneNumber,
                    gender,
                    city
                });
                return res.IsSuccessStatusCode;
            }
            catch { return false; }
        }
    }
}
