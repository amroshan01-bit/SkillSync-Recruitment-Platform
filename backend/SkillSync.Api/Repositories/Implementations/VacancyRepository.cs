using Microsoft.EntityFrameworkCore;
using SkillSync.Api.Data;
using SkillSync.Api.Models;
using SkillSync.Api.Repositories.Interfaces;

namespace SkillSync.Api.Repositories.Implementations;

public class VacancyRepository : IVacancyRepository
{
    private readonly ApplicationDbContext _context;

    public VacancyRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Vacancy>> GetAllAsync(
        string? status,
        string? search)
    {
        var query = _context.Vacancies
            .Include(vacancy => vacancy.EmployerProfile)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(status))
        {
            query = query.Where(
                vacancy => vacancy.Status == status);
        }

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(vacancy =>
                vacancy.JobTitle.Contains(search) ||
                vacancy.Department.Contains(search) ||
                vacancy.Location.Contains(search));
        }

        return await query
            .OrderByDescending(vacancy => vacancy.CreatedAtUtc)
            .ToListAsync();
    }

    public async Task<List<Vacancy>> GetByEmployerProfileIdAsync(
        Guid employerProfileId,
        string? status)
    {
        var query = _context.Vacancies
            .Include(vacancy => vacancy.EmployerProfile)
            .Where(vacancy =>
                vacancy.EmployerProfileId == employerProfileId);

        if (!string.IsNullOrWhiteSpace(status))
        {
            query = query.Where(
                vacancy => vacancy.Status == status);
        }

        return await query
            .OrderByDescending(vacancy => vacancy.CreatedAtUtc)
            .ToListAsync();
    }

    public async Task<Vacancy?> GetByIdAsync(Guid id)
    {
        return await _context.Vacancies
            .Include(vacancy => vacancy.EmployerProfile)
            .FirstOrDefaultAsync(vacancy => vacancy.Id == id);
    }

    public async Task<bool> EmployerProfileExistsAsync(
        Guid employerProfileId)
    {
        return await _context.EmployerProfiles
            .AnyAsync(profile => profile.Id == employerProfileId);
    }

    public async Task<Vacancy> CreateAsync(Vacancy vacancy)
    {
        _context.Vacancies.Add(vacancy);
        await _context.SaveChangesAsync();

        return vacancy;
    }

    public async Task UpdateAsync(Vacancy vacancy)
    {
        _context.Vacancies.Update(vacancy);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Vacancy vacancy)
    {
        _context.Vacancies.Remove(vacancy);
        await _context.SaveChangesAsync();
    }
}