using SkillSync.Api.DTOs.Employer;
using SkillSync.Api.Models;
using SkillSync.Api.Repositories.Interfaces;
using SkillSync.Api.Services.Interfaces;

namespace SkillSync.Api.Services.Implementations;

public class EmployerProfileService : IEmployerProfileService
{
    private readonly IEmployerProfileRepository _repository;

    public EmployerProfileService(
        IEmployerProfileRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<EmployerProfileDto>> GetAllAsync()
    {
        var profiles = await _repository.GetAllAsync();

        return profiles.Select(MapToDto).ToList();
    }

    public async Task<EmployerProfileDto?> GetByIdAsync(Guid id)
    {
        var profile = await _repository.GetByIdAsync(id);

        return profile is null ? null : MapToDto(profile);
    }

    public async Task<EmployerProfileDto?> GetByUserIdAsync(
        Guid userId)
    {
        var profile = await _repository.GetByUserIdAsync(userId);

        return profile is null ? null : MapToDto(profile);
    }

    public async Task<EmployerProfileDto?> CreateAsync(
    Guid userId,
    CreateEmployerProfileDto dto)
    {
        var profileExists =
            await _repository.ExistsForUserAsync(userId);

        if (profileExists)
        {
            return null;
        }

        var profile = new EmployerProfile
        {
            UserId = userId,
            CompanyName = dto.CompanyName,
            Industry = dto.Industry,
            CompanySize = dto.CompanySize,
            Website = dto.Website,
            Location = dto.Location,
            AboutCompany = dto.AboutCompany,
            LogoPath = dto.LogoPath,
            ContactPerson = dto.ContactPerson,
            EmailAddress = dto.EmailAddress,
            PhoneNumber = dto.PhoneNumber
        };

        var createdProfile =
            await _repository.CreateAsync(profile);

        return MapToDto(createdProfile);
    }

    public async Task<bool> UpdateAsync(
    Guid userId,
    UpdateEmployerProfileDto dto)
{
    var profile =
        await _repository.GetByUserIdAsync(userId);
        if (profile is null)
        {
            return false;
        }

        profile.CompanyName = dto.CompanyName;
        profile.Industry = dto.Industry;
        profile.CompanySize = dto.CompanySize;
        profile.Website = dto.Website;
        profile.Location = dto.Location;
        profile.AboutCompany = dto.AboutCompany;
        profile.LogoPath = dto.LogoPath;
        profile.ContactPerson = dto.ContactPerson;
        profile.EmailAddress = dto.EmailAddress;
        profile.PhoneNumber = dto.PhoneNumber;
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

    private static EmployerProfileDto MapToDto(
        EmployerProfile profile)
    {
        return new EmployerProfileDto
        {
            Id = profile.Id,
            UserId = profile.UserId,
            CompanyName = profile.CompanyName,
            Industry = profile.Industry,
            CompanySize = profile.CompanySize,
            Website = profile.Website,
            Location = profile.Location,
            AboutCompany = profile.AboutCompany,
            LogoPath = profile.LogoPath,
            ContactPerson = profile.ContactPerson,
            EmailAddress = profile.EmailAddress,
            PhoneNumber = profile.PhoneNumber,
            CreatedAtUtc = profile.CreatedAtUtc,
            UpdatedAtUtc = profile.UpdatedAtUtc
        };
    }
}