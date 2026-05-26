namespace UserApi.Dtos
{
    public class ProfileDto
    {
        public Guid Id { get; set; }
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? PhoneNumber { get; set; }
        public string? Country { get; set; }
        public string? City { get; set; }
        public string? Gender { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
