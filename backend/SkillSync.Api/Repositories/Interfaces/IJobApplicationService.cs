using SkillSync.Api.DTOs.Application;

namespace SkillSync.Api.Services.Interfaces;

public interface IJobApplicationService
{
    Task<(
        JobApplicationDto? Application,
        string? ErrorMessage)> CreateAsync(
            CreateJobApplicationRequest request);

    Task<List<JobApplicationDto>>
        GetByJobSeekerUserIdAsync(
            Guid jobSeekerUserId);

    Task<List<JobApplicationDto>>
        GetByVacancyIdAsync(Guid vacancyId);

    Task<bool> UpdateStatusAsync(
        Guid applicationId,
        UpdateApplicationStatusRequest request);
}