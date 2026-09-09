using SkillSync.Api.Models;

namespace SkillSync.Api.Repositories.Interfaces;

public interface IJobApplicationRepository
{
    Task<Vacancy?> GetVacancyByIdAsync(Guid vacancyId);

    Task<bool> ApplicationExistsAsync(
        Guid jobSeekerUserId,
        Guid vacancyId);

    Task<JobApplication> CreateAsync(
        JobApplication application);

    Task<List<JobApplication>> GetByJobSeekerUserIdAsync(
        Guid jobSeekerUserId);

    Task<List<JobApplication>> GetByVacancyIdAsync(
        Guid vacancyId);

    Task<JobApplication?> GetByIdAsync(Guid id);

    Task UpdateAsync(JobApplication application);
}