using SkillSync.Api.Models;

namespace SkillSync.Api.Repositories.Interfaces;

public interface IJobSeekerCvRepository
{
    Task<JobSeekerCv?> GetByUserIdAsync(Guid userId);
    Task<JobSeekerCv> CreateAsync(JobSeekerCv cv);
    Task DeleteAsync(JobSeekerCv cv);
}
