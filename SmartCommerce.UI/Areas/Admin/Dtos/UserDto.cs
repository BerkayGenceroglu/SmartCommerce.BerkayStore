namespace SmartCommerce.UI.Areas.Admin.Dtos
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public int Role { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
