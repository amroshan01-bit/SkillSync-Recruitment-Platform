using SkillSync.Api.DTOs.Employer;

namespace SkillSync.Api.Services.Interfaces;

public interface IEmployerProfileService
{
    Task<List<EmployerProfileDto>> GetAllAsync();

    Task<EmployerProfileDto?> GetByIdAsync(Guid id);

    Task<EmployerProfileDto?> GetByUserIdAsync(Guid userId);

    Task<EmployerProfileDto?> CreateAsync(
        CreateEmployerProfileDto dto);

    Task<bool> UpdateAsync(
        Guid id,
        UpdateEmployerProfileDto dto);

    Task<bool> DeleteAsync(Guid id);
}