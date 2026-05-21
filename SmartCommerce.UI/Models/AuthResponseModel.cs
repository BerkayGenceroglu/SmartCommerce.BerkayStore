namespace SmartCommerce.UI.Models
{
    public class AuthResponseModel
    {
        public string Token { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string UserId { get; set; } = null!;

    }
}
