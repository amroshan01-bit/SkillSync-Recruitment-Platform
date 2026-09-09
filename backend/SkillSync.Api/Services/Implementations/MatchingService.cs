using SkillSync.Api.DTOs.Matching;
using SkillSync.Api.Models;
using SkillSync.Api.Repositories.Interfaces;
using SkillSync.Api.Services.Interfaces;

namespace SkillSync.Api.Services.Implementations;

public class MatchingService : IMatchingService
{
    private readonly IMatchingRepository _repository;

    public MatchingService(IMatchingRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<string>> GetSkillsAsync(Guid userId)
    {
        var skills = await _repository.GetSkillsByUserIdAsync(userId);

        return skills
            .Select(skill => skill.SkillName)
            .OrderBy(skillName => skillName)
            .ToList();
    }

    public async Task<bool> UpdateSkillsAsync(
        UpdateJobSeekerSkillsRequest request)
    {
        if (request.UserId == Guid.Empty)
        {
            return false;
        }

        // Remove empty and duplicate skill names.
        var cleanedSkills = request.Skills
            .Where(skill => !string.IsNullOrWhiteSpace(skill))
            .Select(skill => skill.Trim())
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();

        var jobSeekerSkills = cleanedSkills
            .Select(skillName => new JobSeekerSkill
            {
                UserId = request.UserId,
                SkillName = skillName
            })
            .ToList();

        await _repository.ReplaceSkillsAsync(
            request.UserId,
            jobSeekerSkills);

        return true;
    }

    public async Task<List<MatchResultDto>> GetMatchesAsync(
        Guid userId)
    {
        var jobSeekerSkills =
            await _repository.GetSkillsByUserIdAsync(userId);

        var vacancies =
            await _repository.GetOpenVacanciesAsync();

        var applications =
            await _repository.GetApplicationsByUserIdAsync(userId);

        var seekerSkillNames = jobSeekerSkills
            .Select(skill => skill.SkillName.Trim())
            .Where(skill => !string.IsNullOrWhiteSpace(skill))
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

        var applicationLookup = applications
            .GroupBy(application => application.VacancyId)
            .ToDictionary(
                group => group.Key,
                group => group.First());

        var results = new List<MatchResultDto>();

        foreach (var vacancy in vacancies)
        {
            // Split required skills stored as comma-separated text.
            var requiredSkills = ParseSkills(
                vacancy.RequiredSkills);

            var matchedSkills = requiredSkills
                .Where(skill => seekerSkillNames.Contains(skill))
                .ToList();

            var missingSkills = requiredSkills
                .Where(skill => !seekerSkillNames.Contains(skill))
                .ToList();

            // Match score is calculated only in the C# backend.
            var matchScore = requiredSkills.Count == 0
                ? 0
                : Math.Round(
                    (double)matchedSkills.Count /
                    requiredSkills.Count * 100,
                    2);

            applicationLookup.TryGetValue(
                vacancy.Id,
                out var existingApplication);

            results.Add(new MatchResultDto
            {
                VacancyId = vacancy.Id,
                JobTitle = vacancy.JobTitle,
                CompanyName =
                    vacancy.EmployerProfile?.CompanyName
                    ?? string.Empty,
                Location = vacancy.Location,
                WorkplaceType = vacancy.WorkplaceType,
                EmploymentType = vacancy.EmploymentType,
                MinimumSalary = vacancy.MinimumSalary,
                MaximumSalary = vacancy.MaximumSalary,
                Currency = vacancy.Currency,
                MatchScore = matchScore,
                MatchedSkills = matchedSkills,
                MissingSkills = missingSkills,
                HasApplied = existingApplication is not null,
                ApplicationStatus = existingApplication?.Status
            });
        }

        return results
            .OrderByDescending(result => result.MatchScore)
            .ThenBy(result => result.JobTitle)
            .ToList();
    }

    private static List<string> ParseSkills(
        string requiredSkills)
    {
        if (string.IsNullOrWhiteSpace(requiredSkills))
        {
            return new List<string>();
        }

        var separators = new[]
        {
            ',',
            ';',
            '\n',
            '\r'
        };

        return requiredSkills
            .Split(
                separators,
                StringSplitOptions.RemoveEmptyEntries |
                StringSplitOptions.TrimEntries)
            .Where(skill => !string.IsNullOrWhiteSpace(skill))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();
    }
}