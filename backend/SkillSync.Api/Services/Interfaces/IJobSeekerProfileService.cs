using SkillSync.Api.DTOs.JobSeeker;

namespace SkillSync.Api.Services.Interfaces;

public interface IJobSeekerProfileService
{
    Task<JobSeekerProfileDto?> GetByUserIdAsync(Guid userId);

    Task<JobSeekerProfileDto?> CreateAsync(
        Guid userId,
        CreateJobSeekerProfileDto dto);

    Task<bool> UpdateAsync(
        Guid userId,
        UpdateJobSeekerProfileDto dto);

    Task<bool> DeleteAsync(Guid userId);
}