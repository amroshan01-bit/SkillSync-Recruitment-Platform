using SkillSync.Api.Models;

namespace SkillSync.Api.Repositories.Interfaces;

public interface IMatchingRepository
{
    Task<List<JobSeekerSkill>> GetSkillsByUserIdAsync(
        Guid userId);

    Task ReplaceSkillsAsync(
        Guid userId,
        List<JobSeekerSkill> skills);

    Task<List<Vacancy>> GetOpenVacanciesAsync();

    Task<List<JobApplication>> GetApplicationsByUserIdAsync(
        Guid userId);
}