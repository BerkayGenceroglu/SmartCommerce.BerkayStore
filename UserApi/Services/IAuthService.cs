using UserApi.Dtos;

namespace UserApi.Services
{
    public interface IAuthService
    {
        Task<RegisterResponseDto> RegisterAsync(RegisterDto dto);
        Task<AuthResponseDto> LoginAsync(LoginDto dto);
        Task<ProfileDto> GetProfileAsync(Guid userId);
        Task UpdateProfileAsync(Guid userId, UpdateProfileDto dto);
        Task ChangePasswordAsync(Guid userId, ChangePasswordDto dto);
    }
}
