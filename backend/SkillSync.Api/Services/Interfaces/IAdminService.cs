using SkillSync.Api.DTOs.Admin;

namespace SkillSync.Api.Services.Interfaces;

public interface IAdminService
{
    Task<List<AdminUserDto>> GetAllUsersAsync();

    Task<AdminUserDto?> GetUserByIdAsync(Guid userId);

    Task<AdminUserDto?> UpdateUserRoleAsync(
        Guid userId,
        UpdateUserRoleRequestDto request);

    Task<AdminUserDto?> UpdateUserStatusAsync(
        Guid userId,
        UpdateUserStatusRequestDto request);
}