using SkillSync.Api.DTOs.Application;

namespace SkillSync.Api.Services.Interfaces;

public interface IJobApplicationService
{
    Task<(
        JobApplicationDto? Application,
        string? ErrorMessage)> CreateAsync(
            Guid jobSeekerUserId,
            CreateJobApplicationRequest request);

    Task<List<JobApplicationDto>>
        GetByJobSeekerUserIdAsync(
            Guid jobSeekerUserId);

    Task<List<JobApplicationDto>?>
        GetByVacancyIdAsync(
            Guid employerUserId,
            Guid vacancyId);

    Task<List<RankedApplicantDto>?>
        GetRankedApplicantsAsync(
            Guid employerUserId,
            Guid vacancyId);

    Task<bool> UpdateStatusAsync(
        Guid employerUserId,
        Guid applicationId,
        UpdateApplicationStatusRequest request);
}