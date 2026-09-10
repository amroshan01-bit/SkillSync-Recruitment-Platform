using SkillSync.Api.DTOs.JobSeeker;
using SkillSync.Api.Models;
using SkillSync.Api.Repositories.Interfaces;
using SkillSync.Api.Services.Interfaces;

namespace SkillSync.Api.Services.Implementations;

public class JobSeekerProfileService : IJobSeekerProfileService
{
    private readonly IJobSeekerProfileRepository _repository;

    public JobSeekerProfileService(
        IJobSeekerProfileRepository repository)
    {
        _repository = repository;
    }

    public async Task<JobSeekerProfileDto?> GetByUserIdAsync(
        Guid userId)
    {
        var profile = await _repository.GetByUserIdAsync(userId);

        return profile is null ? null : MapToDto(profile);
    }

    public async Task<JobSeekerProfileDto?> CreateAsync(
        Guid userId,
        CreateJobSeekerProfileDto dto)
    {
        var profileExists =
            await _repository.ExistsForUserAsync(userId);

        if (profileExists)
        {
            return null;
        }

        var profile = new JobSeekerProfile
        {
            UserId = userId,
            FullName = dto.FullName,
            ProfessionalTitle = dto.ProfessionalTitle,
            Bio = dto.Bio,
            Location = dto.Location,
            PhoneNumber = dto.PhoneNumber,
            YearsOfExperience = dto.YearsOfExperience,
            HighestQualification = dto.HighestQualification
        };

        var createdProfile =
            await _repository.CreateAsync(profile);

        return MapToDto(createdProfile);
    }

    public async Task<bool> UpdateAsync(
        Guid userId,
        UpdateJobSeekerProfileDto dto)
    {
        var profile =
            await _repository.GetByUserIdAsync(userId);

        if (profile is null)
        {
            return false;
        }

        profile.FullName = dto.FullName;
        profile.ProfessionalTitle = dto.ProfessionalTitle;
        profile.Bio = dto.Bio;
        profile.Location = dto.Location;
        profile.PhoneNumber = dto.PhoneNumber;
        profile.YearsOfExperience = dto.YearsOfExperience;
        profile.HighestQualification = dto.HighestQualification;
        profile.UpdatedAtUtc = DateTime.UtcNow;

        await _repository.UpdateAsync(profile);

        return true;
    }

    public async Task<bool> DeleteAsync(Guid userId)
    {
        var profile =
            await _repository.GetByUserIdAsync(userId);

        if (profile is null)
        {
            return false;
        }

        await _repository.DeleteAsync(profile);

        return true;
    }

    private static JobSeekerProfileDto MapToDto(
        JobSeekerProfile profile)
    {
        return new JobSeekerProfileDto
        {
            Id = profile.Id,
            UserId = profile.UserId,
            FullName = profile.FullName,
            ProfessionalTitle = profile.ProfessionalTitle,
            Bio = profile.Bio,
            Location = profile.Location,
            PhoneNumber = profile.PhoneNumber,
            YearsOfExperience = profile.YearsOfExperience,
            HighestQualification = profile.HighestQualification,
            CreatedAtUtc = profile.CreatedAtUtc,
            UpdatedAtUtc = profile.UpdatedAtUtc
        };
    }
}