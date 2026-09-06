using SkillSync.Api.DTOs.Vacancy;

namespace SkillSync.Api.Services.Interfaces;

public interface IVacancyService
{
    Task<List<VacancyDto>> GetAllAsync(
        string? status,
        string? search);

    Task<List<VacancyDto>> GetByEmployerProfileIdAsync(
        Guid employerProfileId,
        string? status);

    Task<VacancyDto?> GetByIdAsync(Guid id);

    Task<VacancyDto?> CreateAsync(CreateVacancyDto dto);

    Task<bool> UpdateAsync(
        Guid id,
        UpdateVacancyDto dto);

    Task<bool> CloseAsync(Guid id);

    Task<bool> DeleteAsync(Guid id);
}