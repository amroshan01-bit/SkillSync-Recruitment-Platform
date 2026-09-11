using SkillSync.Api.DTOs.Vacancy;
using SkillSync.Api.Models;
using SkillSync.Api.Repositories.Interfaces;
using SkillSync.Api.Services.Interfaces;

namespace SkillSync.Api.Services.Implementations;

public class VacancyService : IVacancyService
{
    private readonly IVacancyRepository _repository;
    private readonly IEmployerProfileRepository
    _employerProfileRepository;

    public VacancyService(
    IVacancyRepository repository,
    IEmployerProfileRepository employerProfileRepository)
{
    _repository = repository;
    _employerProfileRepository = employerProfileRepository;
}

    public async Task<List<VacancyDto>> GetAllAsync(
        string? status,
        string? search)
    {
        var vacancies =
            await _repository.GetAllAsync(status, search);

        return vacancies.Select(MapToDto).ToList();
    }

    public async Task<List<VacancyDto>?>
    GetCurrentEmployerAsync(
        Guid userId,
        string? status)
{
    var employerProfile =
        await _employerProfileRepository
            .GetByUserIdAsync(userId);

    if (employerProfile is null)
    {
        return null;
    }

    var vacancies =
        await _repository.GetByEmployerProfileIdAsync(
            employerProfile.Id,
            status);

    return vacancies.Select(MapToDto).ToList();
}

    public async Task<VacancyDto?> GetByIdAsync(Guid id)
    {
        var vacancy = await _repository.GetByIdAsync(id);

        return vacancy is null ? null : MapToDto(vacancy);
    }

    public async Task<VacancyDto?> CreateAsync(
    Guid userId,
    CreateVacancyDto dto)
    {
        var employerProfile =
    await _employerProfileRepository
        .GetByUserIdAsync(userId);

if (employerProfile is null ||
    dto.MaximumSalary < dto.MinimumSalary ||
    dto.ApplicationClosingDate <= DateTime.UtcNow)
{
    return null;
}

        var vacancy = new Vacancy
        {
            EmployerProfileId = employerProfile.Id,
            JobTitle = dto.JobTitle,
            Department = dto.Department,
            Location = dto.Location,
            WorkplaceType = dto.WorkplaceType,
            EmploymentType = dto.EmploymentType,
            ExperienceLevel = dto.ExperienceLevel,
            JobDescription = dto.JobDescription,
            Requirements = dto.Requirements,
            RequiredSkills = dto.RequiredSkills,
            MinimumSalary = dto.MinimumSalary,
            MaximumSalary = dto.MaximumSalary,
            Currency = dto.Currency,
            ApplicationClosingDate =
                dto.ApplicationClosingDate,
            NumberOfOpenings = dto.NumberOfOpenings,
            Status = dto.Status,
            PostedAtUtc = dto.Status == "Open"
                ? DateTime.UtcNow
                : null
        };

        var createdVacancy =
            await _repository.CreateAsync(vacancy);

        var savedVacancy =
            await _repository.GetByIdAsync(createdVacancy.Id);

        return savedVacancy is null
            ? null
            : MapToDto(savedVacancy);
    }

    public async Task<bool> UpdateAsync(
    Guid userId,
    Guid id,
    UpdateVacancyDto dto)
    {
        var employerProfile =
    await _employerProfileRepository
        .GetByUserIdAsync(userId);

var vacancy = await _repository.GetByIdAsync(id);

if (employerProfile is null ||
    vacancy is null ||
    vacancy.EmployerProfileId != employerProfile.Id ||
    vacancy.Status == "Closed" ||
            dto.MaximumSalary < dto.MinimumSalary ||
            dto.ApplicationClosingDate <= DateTime.UtcNow)
        {
            return false;
        }

        vacancy.JobTitle = dto.JobTitle;
        vacancy.Department = dto.Department;
        vacancy.Location = dto.Location;
        vacancy.WorkplaceType = dto.WorkplaceType;
        vacancy.EmploymentType = dto.EmploymentType;
        vacancy.ExperienceLevel = dto.ExperienceLevel;
        vacancy.JobDescription = dto.JobDescription;
        vacancy.Requirements = dto.Requirements;
        vacancy.RequiredSkills = dto.RequiredSkills;
        vacancy.MinimumSalary = dto.MinimumSalary;
        vacancy.MaximumSalary = dto.MaximumSalary;
        vacancy.Currency = dto.Currency;
        vacancy.ApplicationClosingDate =
            dto.ApplicationClosingDate;
        vacancy.NumberOfOpenings = dto.NumberOfOpenings;

        if (vacancy.Status == "Draft" && dto.Status == "Open")
        {
            vacancy.PostedAtUtc = DateTime.UtcNow;
        }

        vacancy.Status = dto.Status;
        vacancy.UpdatedAtUtc = DateTime.UtcNow;

        await _repository.UpdateAsync(vacancy);

        return true;
    }

    public async Task<bool> CloseAsync(
    Guid userId,
    Guid id)
    {
        var employerProfile =
    await _employerProfileRepository
        .GetByUserIdAsync(userId);

var vacancy = await _repository.GetByIdAsync(id);

if (employerProfile is null ||
    vacancy is null ||
    vacancy.EmployerProfileId != employerProfile.Id ||
    vacancy.Status == "Closed")
        {
            return false;
        }

        vacancy.Status = "Closed";
        vacancy.UpdatedAtUtc = DateTime.UtcNow;

        await _repository.UpdateAsync(vacancy);

        return true;
    }

    public async Task<bool> DeleteAsync(
    Guid userId,
    Guid id)
    {
        var employerProfile =
    await _employerProfileRepository
        .GetByUserIdAsync(userId);

var vacancy = await _repository.GetByIdAsync(id);

if (employerProfile is null ||
    vacancy is null ||
    vacancy.EmployerProfileId != employerProfile.Id)
        {
            return false;
        }

        await _repository.DeleteAsync(vacancy);

        return true;
    }

    private static VacancyDto MapToDto(Vacancy vacancy)
    {
        return new VacancyDto
        {
            Id = vacancy.Id,
            EmployerProfileId = vacancy.EmployerProfileId,
            CompanyName =
                vacancy.EmployerProfile?.CompanyName
                ?? string.Empty,
            JobTitle = vacancy.JobTitle,
            Department = vacancy.Department,
            Location = vacancy.Location,
            WorkplaceType = vacancy.WorkplaceType,
            EmploymentType = vacancy.EmploymentType,
            ExperienceLevel = vacancy.ExperienceLevel,
            JobDescription = vacancy.JobDescription,
            Requirements = vacancy.Requirements,
            RequiredSkills = vacancy.RequiredSkills,
            MinimumSalary = vacancy.MinimumSalary,
            MaximumSalary = vacancy.MaximumSalary,
            Currency = vacancy.Currency,
            ApplicationClosingDate =
                vacancy.ApplicationClosingDate,
            NumberOfOpenings = vacancy.NumberOfOpenings,
            Status = vacancy.Status,
            PostedAtUtc = vacancy.PostedAtUtc,
            CreatedAtUtc = vacancy.CreatedAtUtc,
            UpdatedAtUtc = vacancy.UpdatedAtUtc
        };
    }
}