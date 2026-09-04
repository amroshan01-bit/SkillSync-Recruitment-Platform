using SkillSync.Api.DTOs.JobSeeker;

namespace SkillSync.Api.Services.Interfaces;

public interface IJobSeekerProfileService
{
    Task<List<JobSeekerProfileDto>> GetAllAsync();

    Task<JobSeekerProfileDto?> GetByIdAsync(Guid id);

    Task<JobSeekerProfileDto?> GetByUserIdAsync(Guid userId);

    Task<JobSeekerProfileDto?> CreateAsync(
        CreateJobSeekerProfileDto dto);

    Task<bool> UpdateAsync(
        Guid id,
        UpdateJobSeekerProfileDto dto);

    Task<bool> DeleteAsync(Guid id);
}