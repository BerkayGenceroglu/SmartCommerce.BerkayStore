namespace UserApi.Dtos
{
    public class RegisterResponseDto
    {
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Message { get; set; } = "Kayıt başarılı!";
    }
}
