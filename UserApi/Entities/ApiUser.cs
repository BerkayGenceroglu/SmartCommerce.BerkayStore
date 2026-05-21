using Shared.Enums;

namespace UserApi.Entities
{
    public class ApiUser
    {
        public Guid Id { get; set; }
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public UserRole Role { get; set; } = UserRole.Admin;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
