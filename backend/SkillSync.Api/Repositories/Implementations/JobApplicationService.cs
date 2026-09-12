using Microsoft.EntityFrameworkCore;
using SkillSync.Api.DTOs.Application;
using SkillSync.Api.Models;
using SkillSync.Api.Repositories.Interfaces;
using SkillSync.Api.Services.Interfaces;

namespace SkillSync.Api.Services.Implementations;

public class JobApplicationService : IJobApplicationService
{
    private readonly IJobApplicationRepository _repository;
    private readonly IMatchingRepository _matchingRepository;
    private readonly IJobSeekerProfileRepository _jobSeekerProfileRepository;

    private static readonly Dictionary<string, string> AllowedStatuses =
        new(StringComparer.OrdinalIgnoreCase)
        {
            ["Pending"] = "Pending",
            ["Under Review"] = "Under Review",
            ["Shortlisted"] = "Shortlisted",
            ["Accepted"] = "Accepted",
            ["Rejected"] = "Rejected"
        };

    public JobApplicationService(
        IJobApplicationRepository repository,
        IMatchingRepository matchingRepository,
        IJobSeekerProfileRepository jobSeekerProfileRepository)
    {
        _repository = repository;
        _matchingRepository = matchingRepository;
        _jobSeekerProfileRepository = jobSeekerProfileRepository;
    }

    public async Task<(
        JobApplicationDto? Application,
        string? ErrorMessage)> CreateAsync(
            Guid jobSeekerUserId,
            CreateJobApplicationRequest request)
    {
        if (jobSeekerUserId == Guid.Empty)
        {
            return (null, "A valid job seeker is required.");
        }

        var vacancy =
            await _repository.GetVacancyByIdAsync(request.VacancyId);

        if (vacancy is null)
        {
            return (null, "Vacancy was not found.");
        }

        if (vacancy.Status != "Open")
        {
            return (null, "This vacancy is not open.");
        }

        if (vacancy.ApplicationClosingDate <= DateTime.UtcNow)
        {
            return (null, "The application closing date has passed.");
        }

        var alreadyApplied =
            await _repository.ApplicationExistsAsync(
                jobSeekerUserId,
                request.VacancyId);

        if (alreadyApplied)
        {
            return (null, "You have already applied for this vacancy.");
        }

        var application = new JobApplication
        {
            VacancyId = request.VacancyId,
            JobSeekerUserId = jobSeekerUserId,
            CoverLetter =
                string.IsNullOrWhiteSpace(request.CoverLetter)
                    ? null
                    : request.CoverLetter.Trim(),
            Status = "Pending",
            Vacancy = vacancy
        };

        try
        {
            var createdApplication =
                await _repository.CreateAsync(application);

            return (MapToDto(createdApplication), null);
        }
        catch (DbUpdateException)
        {
            return (
                null,
                "You have already applied for this vacancy.");
        }
    }

    public async Task<List<JobApplicationDto>>
        GetByJobSeekerUserIdAsync(Guid jobSeekerUserId)
    {
        var applications =
            await _repository.GetByJobSeekerUserIdAsync(
                jobSeekerUserId);

        return applications
            .Select(MapToDto)
            .ToList();
    }

    public async Task<List<JobApplicationDto>?>
        GetByVacancyIdAsync(
            Guid employerUserId,
            Guid vacancyId)
    {
        var vacancy =
            await _repository.GetVacancyByIdAsync(vacancyId);

        if (vacancy?.EmployerProfile is null ||
            vacancy.EmployerProfile.UserId != employerUserId)
        {
            return null;
        }

        var applications =
            await _repository.GetByVacancyIdAsync(vacancyId);

        return applications
            .Select(MapToDto)
            .ToList();
    }

    public async Task<List<RankedApplicantDto>?>
        GetRankedApplicantsAsync(
            Guid employerUserId,
            Guid vacancyId)
    {
        var vacancy =
            await _repository.GetVacancyByIdAsync(vacancyId);

        if (vacancy?.EmployerProfile is null ||
            vacancy.EmployerProfile.UserId != employerUserId)
        {
            return null;
        }

        var requiredSkills =
            ParseSkills(vacancy.RequiredSkills);

        var applications =
            await _repository.GetByVacancyIdAsync(vacancyId);

        var rankedApplicants =
            new List<RankedApplicantDto>();

        foreach (var application in applications)
        {
            var seekerSkills =
                await _matchingRepository.GetSkillsByUserIdAsync(
                    application.JobSeekerUserId);

            var seekerSkillNames = seekerSkills
                .Select(skill => skill.SkillName)
                .Where(skill => !string.IsNullOrWhiteSpace(skill))
                .Select(skill => skill.Trim())
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();

            var matchedSkills = requiredSkills
                .Where(required =>
                    seekerSkillNames.Contains(
                        required,
                        StringComparer.OrdinalIgnoreCase))
                .ToList();

            var missingSkills = requiredSkills
                .Where(required =>
                    !seekerSkillNames.Contains(
                        required,
                        StringComparer.OrdinalIgnoreCase))
                .ToList();

            var matchScore =
                requiredSkills.Count == 0
                    ? 0
                    : Math.Round(
                        (double)matchedSkills.Count /
                        requiredSkills.Count * 100,
                        2);

            var profile =
                await _jobSeekerProfileRepository.GetByUserIdAsync(
                    application.JobSeekerUserId);

            rankedApplicants.Add(
                new RankedApplicantDto
                {
                    ApplicationId = application.Id,
                    VacancyId = application.VacancyId,
                    JobSeekerUserId =
                        application.JobSeekerUserId,
                    FullName =
                        profile?.FullName ?? "Job Seeker",
                    ProfessionalTitle =
                        profile?.ProfessionalTitle,
                    Location =
                        profile?.Location,
                    YearsOfExperience =
                        profile?.YearsOfExperience ?? 0,
                    HighestQualification =
                        profile?.HighestQualification,
                    Status = application.Status,
                    CoverLetter = application.CoverLetter,
                    MatchScore = matchScore,
                    MatchedSkills = matchedSkills,
                    MissingSkills = missingSkills,
                    AppliedAtUtc = application.AppliedAtUtc
                });
        }

        return rankedApplicants
            .OrderByDescending(applicant => applicant.MatchScore)
            .ThenBy(applicant => applicant.AppliedAtUtc)
            .ToList();
    }

    public async Task<bool> UpdateStatusAsync(
        Guid employerUserId,
        Guid applicationId,
        UpdateApplicationStatusRequest request)
    {
        var application =
            await _repository.GetByIdAsync(applicationId);

        if (application is null ||
            application.Vacancy?.EmployerProfile is null ||
            application.Vacancy.EmployerProfile.UserId != employerUserId ||
            string.IsNullOrWhiteSpace(request.Status) ||
            !AllowedStatuses.TryGetValue(
                request.Status.Trim(),
                out var validStatus))
        {
            return false;
        }

        application.Status = validStatus;
        application.UpdatedAtUtc = DateTime.UtcNow;

        await _repository.UpdateAsync(application);

        return true;
    }

    private static List<string> ParseSkills(string? skills)
    {
        if (string.IsNullOrWhiteSpace(skills))
        {
            return [];
        }

        return skills
            .Split(
                [',', ';', '\n', '\r'],
                StringSplitOptions.RemoveEmptyEntries)
            .Select(skill => skill.Trim())
            .Where(skill => skill.Length > 0)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();
    }

    private static JobApplicationDto MapToDto(
        JobApplication application)
    {
        return new JobApplicationDto
        {
            Id = application.Id,
            VacancyId = application.VacancyId,
            JobSeekerUserId = application.JobSeekerUserId,
            JobTitle =
                application.Vacancy?.JobTitle
                ?? string.Empty,
            CompanyName =
                application.Vacancy?
                    .EmployerProfile?
                    .CompanyName
                ?? string.Empty,
            Status = application.Status,
            CoverLetter = application.CoverLetter,
            AppliedAtUtc = application.AppliedAtUtc,
            UpdatedAtUtc = application.UpdatedAtUtc
        };
    }
}