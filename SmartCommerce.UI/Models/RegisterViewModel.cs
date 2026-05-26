namespace SmartCommerce.UI.Models
{
    public class RegisterViewModel
    {
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string? PhoneNumber { get; set; }
        public string? Gender { get; set; }
        public string? City { get; set; }
    }
}
