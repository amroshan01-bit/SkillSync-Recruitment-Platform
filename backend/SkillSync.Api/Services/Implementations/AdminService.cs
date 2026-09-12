using SkillSync.Api.DTOs.Admin;
using SkillSync.Api.Models;
using SkillSync.Api.Repositories.Interfaces;
using SkillSync.Api.Services.Interfaces;

namespace SkillSync.Api.Services.Implementations;

public class AdminService : IAdminService
{
    private readonly IUserRepository _userRepository;

    public AdminService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<List<AdminUserDto>> GetAllUsersAsync()
    {
        var users = await _userRepository.GetAllAsync();

        return users
            .Select(MapToDto)
            .ToList();
    }

    public async Task<AdminUserDto?> GetUserByIdAsync(
        Guid userId)
    {
        var user = await _userRepository.GetByIdAsync(userId);

        return user is null
            ? null
            : MapToDto(user);
    }

    public async Task<AdminUserDto?> UpdateUserRoleAsync(
        Guid userId,
        UpdateUserRoleRequestDto request)
    {
        var validRole = GetValidRole(request.Role);

        if (validRole is null)
        {
            throw new ArgumentException(
                "Role must be JobSeeker, Employer, or Admin.");
        }

        var user = await _userRepository.GetByIdAsync(userId);

        if (user is null)
        {
            return null;
        }

        user.Role = validRole;
        user.UpdatedAtUtc = DateTime.UtcNow;

        await _userRepository.UpdateAsync(user);

        return MapToDto(user);
    }

    public async Task<AdminUserDto?> UpdateUserStatusAsync(
        Guid userId,
        UpdateUserStatusRequestDto request)
    {
        var user = await _userRepository.GetByIdAsync(userId);

        if (user is null)
        {
            return null;
        }

        user.IsActive = request.IsActive;
        user.UpdatedAtUtc = DateTime.UtcNow;

        await _userRepository.UpdateAsync(user);

        return MapToDto(user);
    }

    private static string? GetValidRole(string role)
    {
        if (string.Equals(
            role,
            UserRoles.JobSeeker,
            StringComparison.OrdinalIgnoreCase))
        {
            return UserRoles.JobSeeker;
        }

        if (string.Equals(
            role,
            UserRoles.Employer,
            StringComparison.OrdinalIgnoreCase))
        {
            return UserRoles.Employer;
        }

        if (string.Equals(
            role,
            UserRoles.Admin,
            StringComparison.OrdinalIgnoreCase))
        {
            return UserRoles.Admin;
        }

        return null;
    }

    private static AdminUserDto MapToDto(User user)
    {
        return new AdminUserDto
        {
            Id = user.Id,
            FullName = user.FullName,
            Email = user.Email,
            Role = user.Role,
            IsActive = user.IsActive,
            CreatedAtUtc = user.CreatedAtUtc,
            UpdatedAtUtc = user.UpdatedAtUtc
        };
    }
}