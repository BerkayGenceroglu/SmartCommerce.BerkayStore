namespace SmartCommerce.UI.Services
{
    public interface IAuthService
    {
        Task<AuthResult> LoginAsync(string email, string password);
        Task<bool> RegisterAsync(string fullName, string email, string password);
    }
    public class AuthResult
    {
        public bool Success { get; set; }
        public string? Token { get; set; }
        public string? FullName { get; set; }
        public string? Error { get; set; }
        public string? UserId { get; set; }

    }
}
