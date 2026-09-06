using SkillSync.Api.Models;

namespace SkillSync.Api.Repositories.Interfaces;

public interface IJobSeekerProfileRepository
{
    Task<List<JobSeekerProfile>> GetAllAsync();

    Task<JobSeekerProfile?> GetByIdAsync(Guid id);

    Task<JobSeekerProfile?> GetByUserIdAsync(Guid userId);

    Task<bool> ExistsForUserAsync(Guid userId);

    Task<JobSeekerProfile> CreateAsync(JobSeekerProfile profile);

    Task UpdateAsync(JobSeekerProfile profile);

    Task DeleteAsync(JobSeekerProfile profile);
}