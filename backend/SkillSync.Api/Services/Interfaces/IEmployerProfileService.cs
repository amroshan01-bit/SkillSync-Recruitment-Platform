using SkillSync.Api.DTOs.Employer;

namespace SkillSync.Api.Services.Interfaces;

public interface IEmployerProfileService
{
    Task<EmployerProfileDto?> GetByUserIdAsync(
        Guid userId);

    Task<EmployerProfileDto?> CreateAsync(
        Guid userId,
        CreateEmployerProfileDto dto);

    Task<bool> UpdateAsync(
        Guid userId,
        UpdateEmployerProfileDto dto);

    Task<bool> DeleteAsync(Guid userId);
}