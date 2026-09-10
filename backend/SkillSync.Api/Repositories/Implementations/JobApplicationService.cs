using Microsoft.EntityFrameworkCore;
using SkillSync.Api.DTOs.Application;
using SkillSync.Api.Models;
using SkillSync.Api.Repositories.Interfaces;
using SkillSync.Api.Services.Interfaces;

namespace SkillSync.Api.Services.Implementations;

public class JobApplicationService
    : IJobApplicationService
{
    private readonly IJobApplicationRepository _repository;

    private static readonly Dictionary<string, string>
        AllowedStatuses =
            new(StringComparer.OrdinalIgnoreCase)
            {
                ["Pending"] = "Pending",
                ["Under Review"] = "Under Review",
                ["Shortlisted"] = "Shortlisted",
                ["Accepted"] = "Accepted",
                ["Rejected"] = "Rejected"
            };

    public JobApplicationService(
        IJobApplicationRepository repository)
    {
        _repository = repository;
    }

    public async Task<(
        JobApplicationDto? Application,
        string? ErrorMessage)> CreateAsync(
            CreateJobApplicationRequest request)
    {
        var vacancy =
            await _repository.GetVacancyByIdAsync(
                request.VacancyId);

        if (vacancy is null)
        {
            return (
                null,
                "Vacancy was not found.");
        }

        if (vacancy.Status != "Open")
        {
            return (
                null,
                "This vacancy is not open.");
        }

        if (vacancy.ApplicationClosingDate <=
            DateTime.UtcNow)
        {
            return (
                null,
                "The application closing date has passed.");
        }

        var alreadyApplied =
            await _repository.ApplicationExistsAsync(
                request.JobSeekerUserId,
                request.VacancyId);

        if (alreadyApplied)
        {
            return (
                null,
                "You have already applied for this vacancy.");
        }

        var application = new JobApplication
        {
            VacancyId = request.VacancyId,
            JobSeekerUserId =
                request.JobSeekerUserId,
            CoverLetter =
                string.IsNullOrWhiteSpace(
                    request.CoverLetter)
                    ? null
                    : request.CoverLetter.Trim(),
            Status = "Pending",
            Vacancy = vacancy
        };

        try
        {
            var createdApplication =
                await _repository.CreateAsync(application);

            return (
                MapToDto(createdApplication),
                null);
        }
        catch (DbUpdateException)
        {
            // The unique database index prevents
            // simultaneous duplicate applications.
            return (
                null,
                "You have already applied for this vacancy.");
        }
    }

    public async Task<List<JobApplicationDto>>
        GetByJobSeekerUserIdAsync(
            Guid jobSeekerUserId)
    {
        var applications =
            await _repository
                .GetByJobSeekerUserIdAsync(
                    jobSeekerUserId);

        return applications
            .Select(MapToDto)
            .ToList();
    }

    public async Task<List<JobApplicationDto>>
        GetByVacancyIdAsync(Guid vacancyId)
    {
        var applications =
            await _repository.GetByVacancyIdAsync(
                vacancyId);

        return applications
            .Select(MapToDto)
            .ToList();
    }

    public async Task<bool> UpdateStatusAsync(
        Guid applicationId,
        UpdateApplicationStatusRequest request)
    {
        var application =
            await _repository.GetByIdAsync(
                applicationId);

        if (application is null ||
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

    private static JobApplicationDto MapToDto(
        JobApplication application)
    {
        return new JobApplicationDto
        {
            Id = application.Id,
            VacancyId = application.VacancyId,
            JobSeekerUserId =
                application.JobSeekerUserId,
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