using SkillSync.Api.DTOs.Auth;

namespace SkillSync.Api.Services.Interfaces;

public interface IAuthService
{
    Task<AuthResponseDto?> RegisterAsync(
        RegisterRequestDto request);

    Task<AuthResponseDto?> LoginAsync(
        LoginRequestDto request);
}
