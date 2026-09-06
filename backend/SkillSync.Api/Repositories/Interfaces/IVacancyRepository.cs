using SkillSync.Api.Models;

namespace SkillSync.Api.Repositories.Interfaces;

public interface IVacancyRepository
{
    Task<List<Vacancy>> GetAllAsync(
        string? status,
        string? search);

    Task<List<Vacancy>> GetByEmployerProfileIdAsync(
        Guid employerProfileId,
        string? status);

    Task<Vacancy?> GetByIdAsync(Guid id);

    Task<bool> EmployerProfileExistsAsync(
        Guid employerProfileId);

    Task<Vacancy> CreateAsync(Vacancy vacancy);

    Task UpdateAsync(Vacancy vacancy);

    Task DeleteAsync(Vacancy vacancy);
}