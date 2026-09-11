using SkillSync.Api.DTOs.Vacancy;

namespace SkillSync.Api.Services.Interfaces;

public interface IVacancyService
{
    Task<List<VacancyDto>> GetAllAsync(
        string? status,
        string? search);

    Task<List<VacancyDto>?> GetCurrentEmployerAsync(
        Guid userId,
        string? status);

    Task<VacancyDto?> GetByIdAsync(Guid id);

    Task<VacancyDto?> CreateAsync(
        Guid userId,
        CreateVacancyDto dto);

    Task<bool> UpdateAsync(
        Guid userId,
        Guid id,
        UpdateVacancyDto dto);

    Task<bool> CloseAsync(
        Guid userId,
        Guid id);

    Task<bool> DeleteAsync(
        Guid userId,
        Guid id);
}