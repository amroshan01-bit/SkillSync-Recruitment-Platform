using SkillSync.Api.Models;

namespace SkillSync.Api.Repositories.Interfaces;

public interface IEmployerProfileRepository
{
    Task<List<EmployerProfile>> GetAllAsync();

    Task<EmployerProfile?> GetByIdAsync(Guid id);

    Task<EmployerProfile?> GetByUserIdAsync(Guid userId);

    Task<bool> ExistsForUserAsync(Guid userId);

    Task<EmployerProfile> CreateAsync(EmployerProfile profile);

    Task UpdateAsync(EmployerProfile profile);

    Task DeleteAsync(EmployerProfile profile);
}